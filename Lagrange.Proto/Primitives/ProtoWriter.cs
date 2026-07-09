using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Lagrange.Proto.Utility;

namespace Lagrange.Proto.Primitives;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ProtoWriter : IDisposable
{
    private static readonly Vector128<sbyte> Ascend = Vector128.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    
    private const int DefaultGrowthSize = 1024;
    private const int InitialGrowthSize = DefaultGrowthSize >> 4;
    
    private Memory<byte> _memory;
    private IBufferWriter<byte>? _writer;
    
    public int BytesPending { get; private set; }
    
    public long BytesCommitted { get; private set; }
    
    public long BytesWritten => BytesCommitted + BytesPending;

    public ProtoWriter() { }
    
    public ProtoWriter(IBufferWriter<byte> writer)
    {
        _writer = writer;
        _memory = default;
    }

    public void EncodeString(ReadOnlySpan<char> str)
    {
        int count = ProtoHelper.GetVarIntLength(str.Length);
        int min = ProtoHelper.GetVarIntMin(count);
        int max = ProtoHelper.GetVarIntMax(count);
        int utf16Max = ProtoConstants.MaxExpansionFactorWhileTranscoding * str.Length;
        if (_memory.Length - BytesPending < utf16Max + count) Grow(utf16Max + count);
        
        if (str.Length + count > min && utf16Max + count < max) // falls within the range
        {
            BytesPending += count;
            var status = ProtoWriteHelper.ToUtf8(str, _memory.Span[BytesPending..], out int written);
            Debug.Assert(status == OperationStatus.Done);
            BytesPending += written;

            ref byte dest = ref Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(_memory.Span), BytesPending - written - count);
            EncodeVarIntLengthTo(written, ref dest);
        }
        else
        {
            EncodeVarInt(Encoding.UTF8.GetByteCount(str));
            var status = ProtoWriteHelper.ToUtf8(str, _memory.Span[BytesPending..], out int written);
            Debug.Assert(status == OperationStatus.Done);
            BytesPending += written;
        }
    }
    
    public void EncodeBytes(ReadOnlySpan<byte> bytes)
    {
        int length = bytes.Length;
        int count = ProtoHelper.GetVarIntLength(length);
        if (_memory.Length - BytesPending < length) Grow(length + count);
        
        EncodeVarInt(length);
        bytes.CopyTo(_memory.Span[BytesPending..]);
        BytesPending += length;
    }
    
    public void EncodeFixed32<T>(T value) where T : unmanaged, INumber<T>
    {
        if (_memory.Length - BytesPending < 4) Grow(4);
        
        ref byte destination = ref MemoryMarshal.GetReference(_memory.Span);
        Unsafe.As<byte, T>(ref Unsafe.Add(ref destination, BytesPending)) = value;
        BytesPending += 4;
    }
    
    public void EncodeFixed64<T>(T value) where T : unmanaged, INumber<T>
    {
        if (_memory.Length - BytesPending < 8) Grow(8);
        
        ref byte destination = ref MemoryMarshal.GetReference(_memory.Span);
        Unsafe.As<byte, T>(ref Unsafe.Add(ref destination, BytesPending)) = value;
        BytesPending += 8;
    }
    
    public void EncodeVarInt<T>(T value) where T : unmanaged, INumber<T>
    {
        if (_memory.Length - BytesPending >= 10)
        {
            if (value < T.CreateTruncating(0x80))
            {
                Unsafe.Add(ref MemoryMarshal.GetReference(_memory.Span), BytesPending++) = byte.CreateTruncating(value);
                return;
            }
            EncodeVarIntUnsafe(value);
            return;
        }
        EncodeVarIntSlowPath(value);
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void EncodeVarIntSlowPath<T>(T value) where T : unmanaged, INumber<T>
    {
        ulong v = ulong.CreateTruncating(value);
        
        while (v > 127)
        {
            WriteRawByte((byte)((v & 0x7F) | 0x80));
            v >>= 7;
        }
        WriteRawByte((byte)v);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void EncodeVarIntLengthTo(int value, ref byte dest)
    {
        if (value < 0x80) 
        {
            dest = (byte)value;
            return;
        }
        
        ref byte first = ref dest;
        int position = 0;
        
        while (true)
        {
            if (value < 0x80)
            {
                Unsafe.Add(ref first, position) = (byte)value;
                break;
            }
            Unsafe.Add(ref first, position) = (byte)(value | 0x80);
            position++;
            value >>= 7;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteRawByte(byte value)
    {
        if (_memory.Length - BytesPending < 1) Grow(1);
        
        Unsafe.Add(ref MemoryMarshal.GetReference(_memory.Span), BytesPending++) = value;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteRawBytes(ReadOnlySpan<byte> bytes)
    {
        if (_memory.Length - BytesPending < bytes.Length) Grow(bytes.Length);
        
        bytes.CopyTo(_memory.Span[BytesPending..]);
        BytesPending += bytes.Length;
    }
    
    /// <summary>
    /// Max VarInt Bytes for u8: 2
    /// Max VarInt Bytes for u16: 3
    /// Max VarInt Bytes for u32: 5
    /// Max VarInt Bytes for u64: 10
    /// Use sizeof(T) to ensure JIT optimization
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void EncodeVarIntUnsafe<T>(T value) where T : unmanaged, INumberBase<T>
    {
        ulong v = ulong.CreateTruncating(value);

        if (sizeof(T) <= 4)
        {
            ulong stage1 = PackScalar<T>(v);
            int leading = BitOperations.LeadingZeroCount(stage1);
            int bytesNeeded = 8 - ((leading - 1) >> 3);
            
            ulong msbMask = 0xFFFFFFFFFFFFFFFF >> ((8 - bytesNeeded + 1) * 8 - 1);
            ulong merged = stage1 | (0x8080808080808080 & msbMask);
            
            ref byte destination = ref MemoryMarshal.GetReference(_memory.Span);
            Unsafe.As<byte, ulong>(ref Unsafe.Add(ref destination, BytesPending)) = merged;
            BytesPending += bytesNeeded;
        }
        else
        {
            if (Sse2.IsSupported || AdvSimd.Arm64.IsSupported)
            {
                var stage1 = PackVector<T>(v).AsSByte();
                var minimum = Vector128.Create(-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                var exists = Vector128.GreaterThan(stage1, Vector128<sbyte>.Zero) | minimum;
                uint bits = exists.AsByte().ExtractMostSignificantBits();

                byte bytes = (byte)(32 - BitOperations.LeadingZeroCount(bits));
                var mask = Vector128.LessThan(Ascend, Vector128.Create((sbyte)bytes));

                var shift = Sse2.IsSupported ? Sse2.ShiftRightLogical128BitLane(mask, 1) : AdvSimd.ExtractVector128(mask.AsByte(), Vector128<byte>.Zero, 1).AsSByte();
                var msbmask = shift & Vector128.Create((sbyte)-128);
                var merged = stage1 | msbmask;

                ref byte destination = ref MemoryMarshal.GetReference(_memory.Span);
                Unsafe.As<byte, Vector128<sbyte>>(ref Unsafe.Add(ref destination, BytesPending)) = merged;
                BytesPending += bytes;
            }
            else
            {
                ref byte first = ref MemoryMarshal.GetReference(_memory.Span);
                
                while (true)
                {
                    if (v < 0x80)
                    {
                        Unsafe.Add(ref first, BytesPending++) = (byte)v;
                        break;
                    }
                    Unsafe.Add(ref first, BytesPending++) = (byte)((uint)v | 0xFFFFFF80);
                    v >>= 7;
                }
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ulong PackScalar<T>(ulong v) where T : unmanaged, INumberBase<T>
    {
        if (Bmi2.X64.IsSupported)
        {
            return sizeof(T) switch
            {
                sizeof(byte) => Bmi2.X64.ParallelBitDeposit(v, 0x000000000000017f),
                sizeof(ushort) => Bmi2.X64.ParallelBitDeposit(v, 0x0000000000037f7f),
                sizeof(uint) => Bmi2.X64.ParallelBitDeposit(v, 0x0000000f7f7f7f7f),
                _ => throw new NotSupportedException()
            };
        }
        else
        {
            return sizeof(T) switch
            {
                sizeof(byte) => (v & 0x000000000000007f) | ((v & 0x0000000000000080) << 1),
                sizeof(ushort) => (v & 0x000000000000007f) | ((v & 0x0000000000003f80) << 1) | ((v & 0x000000000000c000) << 2),
                sizeof(uint) => (v & 0x000000000000007f) | ((v & 0x0000000000003f80) << 1) | ((v & 0x00000000001fc000) << 2) | ((v & 0x000000000fe00000) << 3) | ((v & 0x00000000f0000000) << 4),
                _ => throw new NotSupportedException()
            };
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe Vector128<ulong> PackVector<T>(ulong v) where T : unmanaged, INumberBase<T>
    {
        if (sizeof(T) < 8) throw new InvalidOperationException("Vector is too small");

        ulong x, y;
        if (Bmi2.X64.IsSupported)
        {
            x = Bmi2.X64.ParallelBitDeposit(v, 0x7f7f7f7f7f7f7f7f);
            y = Bmi2.X64.ParallelBitDeposit(v >> 56, 0x000000000000017f);
        }
        else if (Avx2.IsSupported)
        {
            var b = Vector128.Create(v);
            var c = Sse2.Or(
                Sse2.Or(
                    Avx2.ShiftLeftLogicalVariable(Sse2.And(b, Vector128.Create(0x00000007f0000000ul, 0x000003f800000000ul)), Vector128.Create(4ul, 5ul)),
                    Avx2.ShiftLeftLogicalVariable(Sse2.And(b, Vector128.Create(0x0001fc0000000000ul, 0x00fe000000000000ul)), Vector128.Create(6ul, 7ul))
                ),
                Sse2.Or(
                    Avx2.ShiftLeftLogicalVariable(Sse2.And(b, Vector128.Create(0x000000000000007ful, 0x0000000000003f80ul)), Vector128.Create(0ul, 1ul)),
                    Avx2.ShiftLeftLogicalVariable(Sse2.And(b, Vector128.Create(0x00000000001fc000ul, 0x000000000fe00000ul)), Vector128.Create(2ul, 3ul))
                )
            );
            var d = Sse2.Or(c, Sse2.ShiftRightLogical128BitLane(c, 8));
            x = Sse41.X64.Extract(d, 0);
            y = ((v & 0x7f00000000000000) >> 56) | ((v & 0x8000000000000000) >> 55);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var b = Vector128.Create(v, v);
            var c = (AdvSimd.ShiftLogical(b & Vector128.Create(0x00000007f0000000ul, 0x000003f800000000ul), Vector128.Create(4L, 5L)) | AdvSimd.ShiftLogical(b & Vector128.Create(0x0001fc0000000000ul, 0x00fe000000000000ul), Vector128.Create(6L, 7L))) | (AdvSimd.ShiftLogical(b & Vector128.Create(0x000000000000007ful, 0x0000000000003f80ul), Vector128.Create(0L, 1L)) | AdvSimd.ShiftLogical(b & Vector128.Create(0x00000000001fc000ul, 0x000000000fe00000ul), Vector128.Create(2L, 3L)));
            var d = c | Vector128.Create(c.GetElement(1), 0ul);
            x = d.ToScalar();
            y = ((v & 0x7f00000000000000) >> 56) | ((v & 0x8000000000000000) >> 55);
        }
        else
        {
            x = (v & 0x000000000000007f) | ((v & 0x0000000000003f80) << 1) | ((v & 0x00000000001fc000) << 2) | ((v & 0x000000000fe00000) << 3) | ((v & 0x00000007f0000000) << 4) | ((v & 0x000003f800000000) << 5) | ((v & 0x0001fc0000000000) << 6) | ((v & 0x00fe000000000000) << 7);
            y = ((v & 0x7f00000000000000) >> 56) | ((v & 0x8000000000000000) >> 55);
        }

        return Vector128.Create(x, y);
    }

    /// <summary>
    /// Encodes two 32-bit (or smaller) varint values using SIMD optimizations
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void EncodeTwo32VarIntUnsafe<TT, TU>(TT first, TU second)
        where TT : unmanaged, INumber<TT>
        where TU : unmanaged, INumber<TU>
    {
        if (sizeof(TT) > 4 || sizeof(TU) > 4) throw new NotSupportedException("Both types must be 32-bit or smaller");
        if (_memory.Length - BytesPending < 10) Grow(10);

        ulong v1 = ulong.CreateTruncating(first);
        ulong v2 = ulong.CreateTruncating(second);

        if (v1 < 0x80 && v2 < 0x80)
        {
            ref byte destination = ref MemoryMarshal.GetReference(_memory.Span);
            Unsafe.Add(ref destination, BytesPending) = (byte)v1;
            Unsafe.Add(ref destination, BytesPending + 1) = (byte)v2;
            BytesPending += 2;
            return;
        }

        if (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported)
        {
            EncodeTwo32VarIntSimd<TT, TU>(v1, v2);
        }
        else
        {
            EncodeVarInt(first);
            EncodeVarInt(second);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void EncodeTwo32VarIntSimd<TT, TU>(ulong v1, ulong v2)
        where TT : unmanaged, INumber<TT>
        where TU : unmanaged, INumber<TU>
    {
        ulong stage1 = PackScalar<TT>(v1);
        ulong stage2 = PackScalar<TU>(v2);

        int leading1 = BitOperations.LeadingZeroCount(stage1 | 1); // Ensure at least 1 byte
        int bytes1 = 8 - ((leading1 - 1) >> 3);

        int leading2 = BitOperations.LeadingZeroCount(stage2 | 1); // Ensure at least 1 byte
        int bytes2 = 8 - ((leading2 - 1) >> 3);

        ulong msbMask1 = 0xFFFFFFFFFFFFFFFF >> ((8 - bytes1 + 1) * 8 - 1);
        ulong merged1 = stage1 | (0x8080808080808080 & msbMask1);

        ulong msbMask2 = 0xFFFFFFFFFFFFFFFF >> ((8 - bytes2 + 1) * 8 - 1);
        ulong merged2 = stage2 | (0x8080808080808080 & msbMask2);

        var vec = Vector128.Create(merged1, merged2).AsByte();
        var indices = GetCompactShuffleVector(bytes1, bytes2);
        Vector128<byte> result;
        if (Ssse3.IsSupported)
            result = Ssse3.Shuffle(vec, indices);
        else
            result = AdvSimd.Arm64.VectorTableLookup(vec, indices);

        ref byte destination = ref MemoryMarshal.GetReference(_memory.Span);
        Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref destination, BytesPending)) = result;
        BytesPending += bytes1 + bytes2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<byte> GetCompactShuffleVector(int bytes1, int bytes2)
    {
        const int maxBytes = 8;
        bytes1 = Math.Max(1, Math.Min(bytes1, maxBytes));
        bytes2 = Math.Max(1, Math.Min(bytes2, maxBytes));
        int index = (bytes1 - 1) * maxBytes + (bytes2 - 1);
        return Lookup.EncodeTwoVarIntShuffle[index];
    }

    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int requiredSize)
    {
        Debug.Assert(requiredSize > 0);

        if (_memory.Length == 0)
        {
            FirstCallToGetMemory(requiredSize);
            return;
        }

        int sizeHint = Math.Max(DefaultGrowthSize, requiredSize);

        Debug.Assert(BytesPending != 0);
        Debug.Assert(_writer != null);

        _writer.Advance(BytesPending);
        BytesCommitted += BytesPending;
        BytesPending = 0;

        _memory = _writer.GetMemory(sizeHint);

        if (_memory.Length < sizeHint) ThrowHelper.ThrowInvalidOperationException_NeedLargerSpan();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void FirstCallToGetMemory(int requiredSize)
    {
        Debug.Assert(_memory.Length == 0);
        Debug.Assert(BytesPending == 0);
        Debug.Assert(_writer != null);
        
        int sizeHint = Math.Max(InitialGrowthSize, requiredSize);
        _memory = _writer.GetMemory(sizeHint);

        if (_memory.Length < sizeHint) ThrowHelper.ThrowInvalidOperationException_NeedLargerSpan();
    }
    
    public void Flush()
    {
        CheckNotDisposed();
        _memory = default;
        
        Debug.Assert(_writer != null);
        if (BytesPending != 0)
        {
            _writer.Advance(BytesPending);
            BytesCommitted += BytesPending;
            BytesPending = 0;
        }
    }
    
    public void Reset(IBufferWriter<byte> bufferWriter)
    {
        _writer = bufferWriter ?? throw new ArgumentNullException(nameof(bufferWriter));

        ResetHelper();
    }
    
    internal void ResetAllStateForCacheReuse()
    {
        ResetHelper();

        _writer = null;
    }
    
    private void ResetHelper()
    {
        BytesPending = 0;
        BytesCommitted = 0;
        _memory = default;
    }
    
    private void CheckNotDisposed()
    {
        if (_writer == null)
        {
            ThrowHelper.ThrowObjectDisposedException_ProtoWriter();
        }
    }
    
    public void Dispose()
    {
        if (_writer == null) return;

        Flush();
        ResetHelper();

        _writer = null;
    }
    
    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"BytesCommitted = {BytesCommitted} BytesPending = {BytesPending}";
}
