using System.Security.Cryptography;

namespace Lagrange.Core.Utility.Extension;

internal static class ByteExtension
{
    private static char[] lowerHexMap = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
    private static char[] upperHexMap = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

    public static string Hex(this byte[] bytes, bool lower = false, bool space = false)
        => Hex(bytes.AsSpan(), lower, space);

    public static string Hex(this Span<byte> bytes, bool lower = false, bool space = false)
        => Hex((ReadOnlySpan<byte>)bytes, lower, space);

    public static string Hex(this ReadOnlySpan<byte> bytes, bool lower = false, bool space = false)
        => space ? HexWithSpace(bytes, lower) : HexNoSpace(bytes, lower);

    public static string HexNoSpace(this ReadOnlySpan<byte> bytes, bool lower = false)
    {
        Span<char> result = bytes.Length <= 1024 ? stackalloc char[bytes.Length * 2] : new char[bytes.Length * 2];
        var map = lower ? lowerHexMap : upperHexMap;
        byte mask = 0x0F;

        for (var i = 0; i < bytes.Length; i++)
        {
            result[i * 2] = map[(bytes[i] >> 4) & mask];
            result[i * 2 + 1] = map[bytes[i] & mask];
        }

        return new string(result);
    }

    public static string HexWithSpace(this ReadOnlySpan<byte> bytes, bool lower = false)
    {
        Span<char> result = bytes.Length <= 512 ? stackalloc char[bytes.Length * 3] : new char[bytes.Length * 3];
        byte mask = 0x0F;
        var map = lower ? lowerHexMap : upperHexMap;

        for (var i = 0; i < bytes.Length; i++)
        {
            result[i * 2] = map[(bytes[i] >> 4) & mask];
            result[i * 2 + 1] = map[bytes[i] & mask];
            result[i * 2 + 2] = ' ';
        }

        return new string(result.Slice(0, Math.Max(0, result.Length - 1)));
    }

    public static string Md5(this byte[] bytes, bool lower = false)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(bytes);
        return hash.Hex(lower);
    }
    
    public static async Task<string> Md5Async(this byte[] bytes, bool lower = false)
    {
        using var md5 = MD5.Create();
        using var stream = new MemoryStream(bytes);
        var hash = await md5.ComputeHashAsync(stream);
        return hash.Hex(lower);
    }

    public static string ComputeBlockMd5(this byte[] bytes, int offset, int count, bool lower = false)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(bytes, offset, count);
        return hash.Hex(lower);
    }
    
    public static async Task<string> ComputeBlockMd5Async(this byte[] bytes, int offset, int count, bool lower = false)
    {
        using var md5 = MD5.Create();
        using var stream = new MemoryStream(bytes, offset, count);
        var hash = await md5.ComputeHashAsync(stream);
        return hash.Hex(lower);
    }
}