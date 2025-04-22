using System.Text;

namespace Lagrange.Core.Utility.Binary.Compression;

internal static class ZCompression
{
    public static byte[] ZCompress(ReadOnlySpan<byte> data, ReadOnlySpan<byte> header = default)
    {
        using var stream = new MemoryStream();
        var deflate = Common.Deflate(data);

        stream.Write(header);
        stream.WriteByte(0x78); // Zlib header
        stream.WriteByte(0xDA); // Zlib header

        stream.Write(deflate);
        
        var checksum = Adler32(data);
        stream.Write(checksum);
        
        return stream.ToArray();
    }
    
    public static byte[] ZCompress(string data, ReadOnlySpan<byte> header = default) => ZCompress(Encoding.UTF8.GetBytes(data), header);

    public static byte[] ZDecompress(ReadOnlySpan<byte> data, bool validate = true)
    {
        var checksum = data[^4..];
        
        var inflate = Common.Inflate(data[2..^4]);
        if (validate) return checksum.SequenceEqual(Adler32(inflate)) ? inflate : throw new Exception("Checksum mismatch");
        return inflate;
    }
        
    private static byte[] Adler32(ReadOnlySpan<byte> data)
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