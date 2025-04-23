using System.IO.Compression;

// ReSharper disable once MustUseReturnValue

namespace Lagrange.Core.Utility.Binary.Compression;

internal static class Common
{
    public static byte[] Deflate(ReadOnlySpan<byte> data)
    {
       using var memoryStream = new MemoryStream();
       using var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress);
       deflateStream.Write(data);
       deflateStream.Close();
       return memoryStream.ToArray();
    }
    
    public static byte[] Inflate(ReadOnlySpan<byte> data)
    {
        using var ms = new MemoryStream();
        using var ds = new DeflateStream(ms, CompressionMode.Decompress, true);
        using var os = new MemoryStream();

        ms.Write(data);
        ms.Position = 0;

        ds.CopyTo(os);
        var deflate = new byte[os.Length];
        os.Position = 0;
        os.Read(deflate, 0, deflate.Length);

        return deflate;
    }
}