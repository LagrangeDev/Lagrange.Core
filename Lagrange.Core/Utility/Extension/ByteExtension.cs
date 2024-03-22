using System.Text;
using System.Security.Cryptography;

namespace Lagrange.Core.Utility.Extension;

internal static class ByteExtension
{
    public static string Hex(this byte[] bytes, bool lower = false, bool space = false)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString(lower ? "x2" : "X2"));
            if (space) sb.Append(' ');
        }
        return sb.ToString();
    }
    
    public static string Hex(this Span<byte> bytes, bool lower = false, bool space = false)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString(lower ? "x2" : "X2"));
            if (space) sb.Append(' ');
        }
        return sb.ToString();
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