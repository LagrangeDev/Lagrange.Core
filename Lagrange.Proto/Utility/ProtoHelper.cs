using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization;

namespace Lagrange.Proto.Utility;

public static class ProtoHelper
{
    private static readonly int[] VarIntBoundaries;

    private static readonly int[] VarIntLengths32;
    private static readonly int[] VarIntLengths64;

    static ProtoHelper()
    {
        VarIntBoundaries = new int[5];
        for (int i = 0; i < VarIntBoundaries.Length; i++) VarIntBoundaries[i] = 1 << (7 * i);
        
        VarIntLengths32 = new int[32];
        VarIntLengths64 = new int[64];
        for (int i = 0; i < 32; i++) VarIntLengths32[i] = (((38 - i) * 0b10010010010010011) >> 19) + (i >> 5);
        for (int i = 0; i < 64; i++) VarIntLengths64[i] = (((70 - i) * 0b10010010010010011) >> 19) + (i >> 6);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetVarIntMin(int length) => VarIntBoundaries[length - 1];
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetVarIntMax(int length) => VarIntBoundaries[length] - 1;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint GetTag(int field, WireType wireType) => ((uint)field << 3) | (byte)wireType;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int GetVarIntLength<T>(T value) where T : unmanaged, INumberBase<T>
    {
        if (value == T.Zero) return 1;
        
        if (sizeof(T) <= 4)
        {
            int leadingZeros = BitOperations.LeadingZeroCount(uint.CreateSaturating(value));
            return VarIntLengths32[leadingZeros];
        }
        else
        {
            int leadingZeros = BitOperations.LeadingZeroCount(ulong.CreateSaturating(value));
            return VarIntLengths64[leadingZeros];
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ZigZagEncode<T>(T value) where T : unmanaged, INumber<T>
    {
        return sizeof(T) switch
        {
            1 or 2 or 4 => T.CreateTruncating(EncodeZigZag32(int.CreateSaturating(value))),
            8 => T.CreateTruncating(EncodeZigZag64(long.CreateSaturating(value))),
            _ => sizeof(T) <= 4
                ? T.CreateTruncating(EncodeZigZag32(int.CreateSaturating(value)))
                : T.CreateTruncating(EncodeZigZag64(long.CreateSaturating(value)))
        };
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint EncodeZigZag32(int n) => (uint)((n << 1) ^ (n >> 31));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong EncodeZigZag64(long n) => (ulong)((n << 1) ^ (n >> 63));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ZigZagDecode<T>(T value) where T : unmanaged, INumber<T>
    {
        return sizeof(T) switch
        {
            1 => T.CreateTruncating(DecodeZigZag32(uint.CreateTruncating(value) & 0xFF)),
            2 => T.CreateTruncating(DecodeZigZag32(uint.CreateTruncating(value) & 0xFFFF)),
            4 => T.CreateTruncating(DecodeZigZag32(uint.CreateTruncating(value))),
            8 => T.CreateTruncating(DecodeZigZag64(ulong.CreateTruncating(value))),
            _ => sizeof(T) <= 4
                ? T.CreateTruncating(DecodeZigZag32(uint.CreateTruncating(value)))
                : T.CreateTruncating(DecodeZigZag64(ulong.CreateTruncating(value)))
        };
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int DecodeZigZag32(uint n) => (int)((n >> 1) ^ (uint)(-(int)(n & 1)));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long DecodeZigZag64(ulong n) => (long)((n >> 1) ^ (ulong)(-(long)(n & 1)));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountString(ReadOnlySpan<char> str)
    {
        int length = Encoding.UTF8.GetByteCount(str);
        return GetVarIntLength(length) + length;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountBytes(ReadOnlySpan<byte> str)
    {
        return GetVarIntLength(str.Length) + str.Length;
    }
}
