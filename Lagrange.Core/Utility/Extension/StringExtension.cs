using System.Globalization;

namespace Lagrange.Core.Utility.Extension;

internal static class StringExtension
{
    public static byte[] UnHex(this string hex)
    {
        if (hex.Length % 2 != 0) throw new ArgumentException("Invalid hex string");

        return Convert.FromHexString(hex);
    }
    
    public static byte[] UnHex(this ReadOnlySpan<char> hex)
    {
        if (hex.Length % 2 != 0) throw new ArgumentException("Invalid hex string");

        return Convert.FromHexString(hex);
    }
}