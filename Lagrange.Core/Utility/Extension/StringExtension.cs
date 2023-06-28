using System.Globalization;

namespace Lagrange.Core.Utility.Extension;

internal static class StringExtension
{
    public static byte[] UnHex(this string hex)
    {
        if (hex.Length % 2 != 0) throw new ArgumentException("Invalid hex string");

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2) bytes[i / 2] = byte.Parse(hex.Substring(i, 2), NumberStyles.HexNumber);
        return bytes;
    }
    
    public static byte[] UnHex(this ReadOnlySpan<char> hex)
    {
        if (hex.Length % 2 != 0) throw new ArgumentException("Invalid hex string");

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2) bytes[i / 2] = byte.Parse(hex.Slice(i, 2), NumberStyles.HexNumber);
        return bytes;
    }
}