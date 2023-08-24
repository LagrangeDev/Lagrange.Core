using System.IO.Compression;

namespace Lagrange.Core.Utility.Binary.Compression;

public static class GZip
{
    public static byte[] Inflate(byte[] input)
    {
        using var inputStream = new MemoryStream(input);
        using var outputStream = new MemoryStream();
        var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
        gzipStream.CopyTo(outputStream);
        gzipStream.Close();
        return outputStream.ToArray();
    }
    
    public static byte[] Deflate(byte[] input)
    {
        using var inputStream = new MemoryStream(input);
        using var outputStream = new MemoryStream();
        var gzipStream = new GZipStream(outputStream, CompressionMode.Compress);
        inputStream.CopyTo(gzipStream);
        gzipStream.Close();
        return outputStream.ToArray();
    }
}