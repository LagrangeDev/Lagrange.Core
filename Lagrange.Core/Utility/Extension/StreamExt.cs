using System.Security.Cryptography;

namespace Lagrange.Core.Utility.Extension;

internal static class StreamExt
{
    public static string Md5(this Stream stream, bool lower = false, bool space = false)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(stream);
        
        stream.Seek(0, SeekOrigin.Begin);
        return hash.Hex(lower, space);
    }
    
    public static string Sha1(this Stream stream, bool lower = false, bool space = false)
    {
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(stream);
        
        stream.Seek(0, SeekOrigin.Begin);
        return hash.Hex(lower, space);
    }
}