using System.Buffers;
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

    public static string Hex(this ReadOnlySpan<byte> bytes, bool lower = true, bool space = false)
    {
        var capacity = (space ? bytes.Length * 3 - 1 : bytes.Length * 2);
        char[]? rentArray = null;
        Span<char> buffer = capacity <= 4096 ? stackalloc char[capacity] : (rentArray = ArrayPool<char>.Shared.Rent(capacity)).AsSpan();
        byte mask = 0x0F;
        var map = lower ? lowerHexMap : upperHexMap;

        if (!space)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                buffer[i * 2] = map[(bytes[i] >> 4) & mask];
                buffer[i * 2 + 1] = map[bytes[i] & mask];
            }
        }
        else
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                buffer[i * 3] = map[(bytes[i] >> 4) & mask];
                buffer[i * 3 + 1] = map[bytes[i] & mask];
            }
            for (var i = 2; i < buffer.Length; i += 3)
                buffer[i] = ' ';
        }

        var str = new string(buffer);
        if (rentArray != null)
            ArrayPool<char>.Shared.Return(rentArray);
        return str;
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