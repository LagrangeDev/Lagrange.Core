using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;

namespace Lagrange.Core.Utility.Binary.Compression;

internal static class ZCompression
{
    public static byte[] ZCompress(byte[] data, byte[]? header = null)
    {
        using var stream = new MemoryStream();
        var deflate = Common.Deflate(data);

        stream.Write(header);
        stream.WriteByte(0x78); // Zlib header
        stream.WriteByte(0xDA); // Zlib header

        stream.Write(deflate.AsSpan());
        
        var checksum = Adler32(data);
        stream.Write(checksum.AsSpan());
        
        return stream.ToArray();
    }
    
    public static byte[] ZCompress(string data, byte[]? header = null) => ZCompress(Encoding.UTF8.GetBytes(data), header);
    
    [SuppressMessage("ReSharper", "MustUseReturnValue")]
    public static byte[] ZDecompress(byte[] data)
    {
        using var stream = new MemoryStream(data);
        var header = new byte[2];
        stream.Read(header);
        if (header[0] != 0x78 || header[1] != 0xDA) throw new InvalidDataException("Invalid Zlib header");
        
        var deflate = new byte[stream.Length - stream.Position - 4];
        stream.Read(deflate);
        
        var checksum = new byte[4];
        stream.Read(checksum);
        
        var decompressed = Common.Inflate(deflate);
        var checksum2 = Adler32(decompressed);
        if (!checksum.AsSpan().SequenceEqual(checksum2.AsSpan())) throw new InvalidDataException("Invalid Zlib checksum");
        
        return decompressed;
    }
    
    private static byte[] Adler32(byte[] data)
    {
        uint a = 1, b = 0;
        foreach (byte t in data)
        {
            a = (a + t) % 65521;
            b = (b + a) % 65521;
        }
        return BitConverter.GetBytes((b << 16) | a, false);
    }
}