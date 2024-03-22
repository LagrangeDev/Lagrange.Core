using SysBitConverter = System.BitConverter;

namespace Lagrange.Core.Utility.Binary;

/// <summary>
/// Provide the same APIs as System.BitConverter, but supports both BigEndian and LittleEndian
/// </summary>
internal static class BitConverter
{
    public static byte[] GetBytes(long value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.GetBytes(value) : new[] { (byte)(value >> 56), (byte)(value >> 48), (byte)(value >> 40), (byte)(value >> 32), (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
    
    public static int ToInt32(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToInt32(value) : (value[0] << 24) | (value[1] << 16) | (value[2] << 8) | value[3];
    
    public static ushort ToUInt16(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToUInt16(value) : (ushort)((value[0] << 8) | value[1]);
    
    public static uint ToUInt32(ReadOnlySpan<byte> value, bool isLittleEndian = true)
        => isLittleEndian ? SysBitConverter.ToUInt32(value) : ((uint)value[0] << 24) | ((uint)value[1] << 16) | ((uint)value[2] << 8) | value[3];
}