using System.IO.Compression;

namespace Lagrange.Core.Utility.Binary.Compression;

internal static class Common
{
    public static byte[] Deflate(byte[] data)
    {
       using var memoryStream = new MemoryStream();
       using var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress);
       deflateStream.Write(data, 0, data.Length);
       deflateStream.Close();
       return memoryStream.ToArray();
    }
    
    public static byte[] Inflate(byte[] data)
    {
       using var memoryStream = new MemoryStream();
       using var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
       deflateStream.Write(data, 0, data.Length);
       deflateStream.Close();
       return memoryStream.ToArray();
    } }