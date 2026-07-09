using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using Lagrange.Proto.Serialization;

namespace Lagrange.Proto.Primitives;

[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public ref struct ProtoReader
{
    private readonly ref byte _first;
    private int _offset;
    private int _length;
    
    private static readonly Vector128<ulong> Mask1 = Vector128.Create(0x000000000000007ful, 0x7f00000000000000ul);
    private static readonly Vector128<ulong> Mask2 = Vector128.Create(0x007f000000000000ul, 0x00007f0000000000ul);
    private static readonly Vector128<ulong> Mask3 = Vector128.Create(0x0000007f00000000ul, 0x000000007f000000ul);
    private static readonly Vector128<ulong> Mask4 = Vector128.Create(0x00000000007f0000ul, 0x0000000000007f00ul);
    
    private static readonly Vector128<ulong> Shift1 = Vector128.Create(0ul, 7ul);
    private static readonly Vector128<ulong> Shift2 = Vector128.Create(6ul, 5ul);
    private static readonly Vector128<ulong> Shift3 = Vector128.Create(4ul, 3ul);
    private static readonly Vector128<ulong> Shift4 = Vector128.Create(2ul, 1ul);
    
    private static readonly Vector128<sbyte> Ascend = Vector128.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    
    public bool IsCompleted => _offset == _length;

    public ProtoReader(ReadOnlySpan<byte> src)
    {
        _first = ref MemoryMarshal.GetReference(src);
        _offset = 0;
        _length = src.Length;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T DecodeVarInt<T>() where T : unmanaged, INumber<T>
    {
        if (_length - _offset < 16)
        {
            return DecodeVarIntSlowPath<T>();
        }
        
        return DecodeVarIntUnsafe<T>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T DecodeFixed32<T>() where T : unmanaged, INumber<T>
    {
        if (_length - _offset < 4) ThrowHelper.ThrowArgumentOutOfRangeException_NoEnoughSpace(nameof(DecodeFixed32),4, _length - _offset);
        
        var result = Unsafe.ReadUnaligned<T>(ref Unsafe.Add(ref _first, _offset));
        _offset += 4;
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T DecodeFixed64<T>() where T : unmanaged, INumber<T>
    {
        if (_length - _offset < 8) ThrowHelper.ThrowArgumentOutOfRangeException_NoEnoughSpace(nameof(DecodeFixed64),8, _length - _offset);
        
        var result = Unsafe.ReadUnaligned<T>(ref Unsafe.Add(ref _first, _offset));
        _offset += 8;
        return result;
    }
    
    public ReadOnlySpan<byte> CreateSpan(int length)
    {
        if (_length - _offset < length) ThrowHelper.ThrowArgumentOutOfRangeException_NoEnoughSpace(nameof(CreateSpan), length, _length - _offset);
        
        var result = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref _first, _offset), length);
        _offset += length;
        return result;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private unsafe T DecodeVarIntSlowPath<T>() where T : unmanaged, INumber<T>
    {
        int shift = 0;
        T result = T.Zero;
        do
        {
            byte b = Unsafe.Add(ref _first, _offset++);
            result += T.CreateTruncating((ulong)(b & 0x7Fu) << shift);
            if (b <= 0x7F)
            {
                return result;
            }
            shift += 7;
        }
        while (shift < sizeof(T) << 3);
        ThrowHelper.ThrowInvalidDataException_MalformedMessage();
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Rewind(int count) => _offset += count;
    
    /// <summary>
    /// Max VarInt Bytes for u8: 2
    /// Max VarInt Bytes for u16: 3
    /// Max VarInt Bytes for u32: 5
    /// Max VarInt Bytes for u64: 10
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T DecodeVarIntUnsafe<T>() where T : unmanaged, INumber<T>
    {
        if (sizeof(T) <= 4) // indicate that the byte can be handled in the 64-bit reg
        {
            ulong b = Unsafe.As<byte, ulong>(ref Unsafe.Add(ref _first, _offset));
            ulong msbs = ~b & ~0x7f7f7f7f7f7f7f7ful;
            ulong varintPart = b & (msbs ^ msbs - 1);

            _offset += (BitOperations.TrailingZeroCount(msbs) + 1) >> 3; // in bits

            if (Bmi2.X64.IsSupported)
            {
                return sizeof(T) switch
                {
                    sizeof(byte) => T.CreateTruncating(Bmi2.X64.ParallelBitExtract(varintPart, 0x000000000000017f)),
                    sizeof(ushort) => T.CreateTruncating(Bmi2.X64.ParallelBitExtract(varintPart, 0x0000000000037f7f)),
                    sizeof(uint) => T.CreateTruncating(Bmi2.X64.ParallelBitExtract(varintPart, 0x0000000f7f7f7f7f)),
                    _ => throw new NotSupportedException()
                };
            }
            else
            {
                return sizeof(T) switch
                {
                    sizeof(byte) => T.CreateTruncating((varintPart & 0x000000000000007f) | ((varintPart & 0x0000000000000100) >> 1)),
                    sizeof(ushort) => T.CreateTruncating((varintPart & 0x000000000000007f) | ((varintPart & 0x0000000000030000) >> 2) | ((varintPart & 0x0000000000007f00) >> 1)),
                    sizeof(uint) => T.CreateTruncating((varintPart & 0x000000000000007f) | ((varintPart & 0x0000000f00000000) >> 4) | ((varintPart & 0x000000007f000000) >> 3) | ((varintPart & 0x00000000007f0000) >> 2) | ((varintPart & 0x0000000000007f00) >> 1)),
                    _ => throw new NotSupportedException()
                };
            }
            
        }
        else
        {
            ulong b0 = Unsafe.As<byte, ulong>(ref Unsafe.Add(ref _first, _offset));
            ulong b1 = Unsafe.As<byte, ulong>(ref Unsafe.Add(ref _first, _offset + 8));
            
            ulong msbs0 = ~b0 & ~0x7f7f7f7f7f7f7f7ful;
            ulong msbs1 = ~b1 & ~0x7f7f7f7f7f7f7f7ful;
            
            int len0 = BitOperations.TrailingZeroCount(msbs0) + 1; // in bits
            ulong varintPart0 = b0 & (msbs0 ^ msbs0 - 1);
            ulong varintPart1;
            
            if (msbs0 == 0)
            {
                varintPart1 = b1 & (msbs1 ^ msbs1 - 1);
                _offset += (BitOperations.TrailingZeroCount(msbs1) + 1 + 64) >> 3;
            }
            else
            {
                varintPart1 = 0;
                _offset += len0 >> 3;
            }
            
            return ExtractFromVector<T>(varintPart0, varintPart1);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T ExtractFromVector<T>(ulong varintPart0, ulong varintPart1) where T : unmanaged, INumber<T>
    {
        if (Bmi2.X64.IsSupported)
        {
            return T.CreateTruncating(Bmi2.X64.ParallelBitExtract(varintPart0, 0x7f7f7f7f7f7f7f7f) | Bmi2.X64.ParallelBitExtract(varintPart1, 0x000000000000017f) << 56);
        }
        else if (Avx2.X64.IsSupported)
        {
            var b = Vector128.Create(varintPart0, varintPart1);
            var c = Avx2.BroadcastScalarToVector128(b);
            var d = Sse2.Or(
                Sse2.Or(
                    Avx2.ShiftRightLogicalVariable(Sse2.And(c, Mask1), Shift1),
                    Avx2.ShiftRightLogicalVariable(Sse2.And(c, Mask2), Shift2)
                ),
                Sse2.Or(
                    Avx2.ShiftRightLogicalVariable(Sse2.And(c, Mask3), Shift3),
                    Avx2.ShiftRightLogicalVariable(Sse2.And(c, Mask4), Shift4)
                )
            );
            var e = Sse2.Or(d, Sse2.ShiftRightLogical128BitLane(d, 8));
            ulong pt1 = Sse41.X64.Extract(e, 0);
                
            return T.CreateTruncating(pt1 | ((varintPart1 & 0x0000000000000100) << 55) | ((varintPart1 & 0x000000000000007f) << 56));
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var b = Vector128.Create(varintPart0, varintPart0);
            var d = AdvSimd.ShiftLogical(b & Mask1, -Shift1.AsInt64()) | AdvSimd.ShiftLogical(b & Mask2, -Shift2.AsInt64()) | AdvSimd.ShiftLogical(b & Mask3, -Shift3.AsInt64()) | AdvSimd.ShiftLogical(b & Mask4, -Shift4.AsInt64());
            var e = d | Vector128.Create(d.GetElement(1), 0ul);
            ulong pt1 = e.ToScalar();

            return T.CreateTruncating(pt1 | ((varintPart1 & 0x0000000000000100) << 55) | ((varintPart1 & 0x000000000000007f) << 56));
        }
        else
        {
            return T.CreateTruncating((varintPart0 & 0x000000000000007f) | ((varintPart0 & 0x7f00000000000000) >> 7) | ((varintPart0 & 0x007f000000000000) >> 6) | ((varintPart0 & 0x00007f0000000000) >> 5) | ((varintPart0 & 0x0000007f00000000) >> 4) | ((varintPart0 & 0x000000007f000000) >> 3) | ((varintPart0 & 0x00000000007f0000) >> 2) | ((varintPart0 & 0x0000000000007f00) >> 1) | ((varintPart1 & 0x0000000000000100) << 55) | ((varintPart1 & 0x000000000000007f) << 56));
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe (TT, TU) DecodeVarIntUnsafe<TT, TU>(ReadOnlySpan<byte> src) 
        where TT : unmanaged, INumber<TT> 
        where TU : unmanaged, INumber<TU>
    {
        if (!Ssse3.X64.IsSupported && !AdvSimd.Arm64.IsSupported) throw new PlatformNotSupportedException();
        
        if (sizeof(TT) + sizeof(TU) > 12) throw new NotSupportedException();

        if (sizeof(TT) <= 4 && sizeof(TU) <= 4) return DecodeTwo32VarIntUnsafe<TT, TU>(src); // try to use fast path of lookup table
        
        var b = Unsafe.As<byte, Vector128<sbyte>>(ref MemoryMarshal.GetReference(src));
        uint bitmask = b.AsByte().ExtractMostSignificantBits();
        uint maskNot = ~bitmask;
        int firstLen = BitOperations.TrailingZeroCount(maskNot) + 1;
        uint maskNot2 = maskNot >> firstLen;
        int secondLen = BitOperations.TrailingZeroCount(maskNot2) + 1;

        var firstLenVec = Vector128.Create((sbyte)firstLen);
        var firstMask = Vector128.LessThan(Ascend, firstLenVec);
        var first = b & firstMask;

        var secondShuf = Ascend + firstLenVec;
        var secondShuffled = Ssse3.IsSupported ? Ssse3.Shuffle(b, secondShuf) : AdvSimd.Arm64.VectorTableLookup(b.AsByte(), secondShuf.AsByte()).AsSByte();
        var secondMask = Vector128.LessThan(Ascend, Vector128.Create((sbyte)secondLen));
        var second = secondShuffled & secondMask;
        
        TT firstNum;
        TU secondNum;
        if (sizeof(TT) <= 4 && sizeof(TU) <= 4 && !Bmi2.X64.IsSupported)
        {
            var shifted = Sse2.IsSupported ? Sse2.ShiftLeftLogical128BitLane(second, 8) : Vector128.Create(0L, second.AsInt64().ToScalar()).AsSByte();
            var comb = (first | shifted).AsUInt64();
            var x = sizeof(TT) <= 1 && sizeof(TU) <= 1 ? DualU8Stage2(comb) : sizeof(TT) <= 2 && sizeof(TU) <= 2 ? DualU16Stage2(comb) : DualU32Stage2(comb);
            firstNum = TT.CreateTruncating(x.GetElement(0));
            secondNum = TU.CreateTruncating(x.GetElement(1));
        }
        else
        {
            firstNum = ExtractFromVector<TT>(Unsafe.As<Vector128<sbyte>, ulong>(ref first), Unsafe.As<Vector128<sbyte>, ulong>(ref Unsafe.AddByteOffset(ref first, 8)));
            secondNum = ExtractFromVector<TU>(Unsafe.As<Vector128<sbyte>, ulong>(ref second), Unsafe.As<Vector128<sbyte>, ulong>(ref Unsafe.AddByteOffset(ref second, 8)));
        }
        
        _offset += (firstLen + secondLen) >> 3; // in bits
        
        return (firstNum, secondNum);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ulong> DualU8Stage2(Vector128<ulong> comb)
    {
        var mask = Vector128.Create(0x000000000000007ful, 0x000000000000007ful);
        return (comb & mask) | Vector128.ShiftRightLogical(comb & mask, 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ulong> DualU16Stage2(Vector128<ulong> comb)
    {
        return ((comb & Vector128.Create(0x000000000000007ful, 0x000000000000007ful)) |
                Vector128.ShiftRightLogical(comb & Vector128.Create(0x0000000000030000ul, 0x0000000000030000ul), 2)) |
               Vector128.ShiftRightLogical(comb & Vector128.Create(0x0000000000007f00ul, 0x0000000000007f00ul), 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ulong> DualU32Stage2(Vector128<ulong> comb)
    {
        return ((comb & Vector128.Create(0x000000000000007ful, 0x000000000000007ful)) |
                Vector128.ShiftRightLogical(comb & Vector128.Create(0x0000000f00000000ul, 0x0000000f00000000ul), 4)) |
               ((Vector128.ShiftRightLogical(comb & Vector128.Create(0x000000007f000000ul, 0x000000007f000000ul), 3) |
                 Vector128.ShiftRightLogical(comb & Vector128.Create(0x00000000007f0000ul, 0x00000000007f0000ul), 2)) |
                Vector128.ShiftRightLogical(comb & Vector128.Create(0x0000000000007f00ul, 0x0000000000007f00ul), 1));
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe (TT, TU) DecodeTwo32VarIntUnsafe<TT, TU>(ReadOnlySpan<byte> src)
        where TT : unmanaged, INumber<TT>
        where TU : unmanaged, INumber<TU>
    {
        var b = Unsafe.As<byte, Vector128<sbyte>>(ref MemoryMarshal.GetReference(src));
        uint bitmask = b.AsByte().ExtractMostSignificantBits() & 0b1111111111;
        var (lookup, firstLen, secondLen) = Lookup.DoubleStep1[(int)bitmask];
        var shuf = Unsafe.Add(ref MemoryMarshal.GetReference(Lookup.DoubleVec), lookup);

        Vector128<ulong> comb;
        if (Ssse3.IsSupported)
            comb = Ssse3.Shuffle(b, shuf).AsUInt64();
        else
            comb = AdvSimd.Arm64.VectorTableLookup(b.AsByte(), shuf.AsByte()).AsUInt64();

        TT firstNum;
        TU secondNum;

        if (Bmi2.X64.IsSupported)
        {
            var shift = Sse2.ShiftRightLogical128BitLane(comb, 8);

            firstNum = ExtractFromVector<TT>(comb[0], comb[1]);
            secondNum = ExtractFromVector<TU>(shift[0], shift[1]);
        }
        else
        {
            var x = sizeof(TT) <= 1 && sizeof(TU) <= 1 ? DualU8Stage2(comb) : sizeof(TT) <= 2 && sizeof(TU) <= 2 ? DualU16Stage2(comb) : DualU32Stage2(comb);
            firstNum = TT.CreateTruncating(x.GetElement(0));
            secondNum = TU.CreateTruncating(x.GetElement(1));
        }
        
        _offset += (firstLen + secondLen) >> 3; // in bits
        
        return (firstNum, secondNum);
    }

    public void SkipField(WireType wireType)
    {
        switch (wireType)
        {
            case WireType.Fixed32:
                SkipFixed32();
                break;
            case WireType.Fixed64:
                SkipFixed64();
                break;
            case WireType.VarInt:
                SkipVarInt();
                break;
            case WireType.LengthDelimited:
                SkipLengthDelimited();
                break;
            case (WireType)3: // Start group (deprecated)
            case (WireType)4: // End group (deprecated)
                // Skip deprecated group markers
                break;
            case (WireType)6: // Reserved
            case (WireType)7: // Reserved
                // Skip reserved wire types
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(wireType), wireType, null);
        }
    }

    private void SkipVarInt()
    {
        ulong b0 = Unsafe.As<byte, ulong>(ref Unsafe.Add(ref _first, _offset));
        ulong b1 = Unsafe.As<byte, ulong>(ref Unsafe.Add(ref _first, _offset + 8));
        ulong msbs0 = ~b0 & ~0x7f7f7f7f7f7f7f7ful;
        ulong msbs1 = ~b1 & ~0x7f7f7f7f7f7f7f7ful;
        _offset += msbs0 == 0 ? (BitOperations.TrailingZeroCount(msbs1) + 1 + 64) >> 3 : (BitOperations.TrailingZeroCount(msbs0) + 1) >> 3;
    }
    
    private void SkipFixed32()
    {
        if (_length - _offset < 4) ThrowHelper.ThrowArgumentOutOfRangeException_NoEnoughSpace(nameof(SkipFixed32), 4, _length - _offset);
        
        _offset += sizeof(uint);
    }
    
    private void SkipFixed64()
    {
        if (_length - _offset < 8) ThrowHelper.ThrowArgumentOutOfRangeException_NoEnoughSpace(nameof(SkipFixed64), 8, _length - _offset);
        
        _offset += sizeof(ulong);
    }
    
    private void SkipLengthDelimited()
    {
        int length = DecodeVarInt<int>();
        if (_length - _offset < length) ThrowHelper.ThrowArgumentOutOfRangeException_NoEnoughSpace(nameof(SkipLengthDelimited), length, _length - _offset);
        
        _offset += length;
    }
    
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"Offset: {_offset}, Length: {_length}, Hex : {Convert.ToHexString(MemoryMarshal.CreateSpan(ref _first, _length))}";
}
