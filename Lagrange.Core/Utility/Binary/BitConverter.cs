using SysBitConverter = System.BitConverter;

namespace Lagrange.Core.Utility.Binary;

/// <summary>
/// Provide the same APIs as System.BitConverter, but supports both BigEndian and LittleEndian
/// </summary>
internal static class BitConverter
{
    public static byte[] GetBytes(short value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 8), (byte)value };

    public static byte[] GetBytes(ushort value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 8), (byte)value };

    public static byte[] GetBytes(int value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };

    public static byte[] GetBytes(uint value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };

    public static byte[] GetBytes(long value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 56), (byte)(value >> 48), (byte)(value >> 40), (byte)(value >> 32), (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };

    public static byte[] GetBytes(ulong value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 56), (byte)(value >> 48), (byte)(value >> 40), (byte)(value >> 32), (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };

    public static short ToInt16(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToInt16(value) : (short)((value[0] << 8) | value[1]);

    public static int ToInt32(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToInt32(value) : (value[0] << 24) | (value[1] << 16) | (value[2] << 8) | value[3];

    public static long ToInt64(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToInt64(value) : ((long)value[0] << 56) | ((long)value[1] << 48) | ((long)value[2] << 40) | ((long)value[3] << 32) | ((long)value[4] << 24) | ((long)value[5] << 16) | ((long)value[6] << 8) | value[7];

    public static ushort ToUInt16(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToUInt16(value) : (ushort)((value[0] << 8) | value[1]);

    public static uint ToUInt32(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToUInt32(value) : ((uint)value[0] << 24) | ((uint)value[1] << 16) | ((uint)value[2] << 8) | value[3];

    public static ulong ToUInt64(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToUInt64(value) : ((ulong)value[0] << 56) | ((ulong)value[1] << 48) | ((ulong)value[2] << 40) | ((ulong)value[3] << 32) | ((ulong)value[4] << 24) | ((ulong)value[5] << 16) | ((ulong)value[6] << 8) | value[7];

    public static float ToSingle(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToSingle(value) : ToSingleBigEndian(value);

    public static double ToDouble(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToDouble(value) : ToDoubleBigEndian(value);

    private static unsafe float ToSingleBigEndian(ReadOnlySpan<byte> value)
    {
        var tmp = ToUInt32(value, false);
        return *(float*)&tmp;
    }

    private static unsafe double ToDoubleBigEndian(ReadOnlySpan<byte> value)
    {
        var tmp = ToUInt64(value, false);
        return *(double*)&tmp;
    }

    public static byte[] GetBytes(float value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : GetBytesBigEndian(value);

    public static byte[] GetBytes(double value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : GetBytesBigEndian(value);

    private static unsafe byte[] GetBytesBigEndian(float value)
    {
        var tmp = *(uint*)&value;
        return GetBytes(tmp, false);
    }

    private static unsafe byte[] GetBytesBigEndian(double value)
    {
        var tmp = *(ulong*)&value;
        return GetBytes(tmp, false);
    }

    public static byte[] GetBytes(bool value)
        => SysBitConverter.GetBytes(value);

    public static bool ToBoolean(ReadOnlySpan<byte> value)
        => SysBitConverter.ToBoolean(value);

    public static byte[] GetBytes(char value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 8), (byte)value };

    public static char ToChar(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToChar(value) : (char)((value[0] << 8) | value[1]);
}