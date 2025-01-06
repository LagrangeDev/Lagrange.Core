using System.Buffers.Binary;

namespace Lagrange.Core.Utility.Crypto.Provider.Tea;

internal static class TeaProvider
{
    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        uint a = BinaryPrimitives.ReadUInt32BigEndian(key.AsSpan(0));
        uint b = BinaryPrimitives.ReadUInt32BigEndian(key.AsSpan(4));
        uint c = BinaryPrimitives.ReadUInt32BigEndian(key.AsSpan(8));
        uint d = BinaryPrimitives.ReadUInt32BigEndian(key.AsSpan(12));

        int fill = 10 - ((data.Length + 1) & 7);
        var result = new byte[fill + data.Length + 7];
        Random.Shared.NextBytes(result.AsSpan(0, fill));
        result[0] = (byte)((fill - 3) | 0xF8);
        data.CopyTo(result.AsSpan(fill));

        ulong plainXor = 0, prevXor = 0;

        for (int i = 0; i < result.Length; i += 8)
        {
            ulong plain = BinaryPrimitives.ReadUInt64BigEndian(result.AsSpan(i)) ^ plainXor;
            uint x = (uint)(plain >> 32);
            uint y = (uint)(plain);

            x += (y + 0x9e3779b9) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x9e3779b9) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x3c6ef372) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x3c6ef372) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0xdaa66d2b) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0xdaa66d2b) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x78dde6e4) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x78dde6e4) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x1715609d) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x1715609d) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0xb54cda56) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0xb54cda56) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x5384540f) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x5384540f) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0xf1bbcdc8) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0xf1bbcdc8) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x8ff34781) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x8ff34781) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x2e2ac13a) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x2e2ac13a) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0xcc623af3) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0xcc623af3) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x6a99b4ac) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x6a99b4ac) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x08d12e65) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x08d12e65) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0xa708a81e) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0xa708a81e) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0x454021d7) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0x454021d7) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x += (y + 0xe3779b90) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y += (x + 0xe3779b90) ^ ((x << 4) + c) ^ ((x >> 5) + d);

            plainXor = ((ulong)x << 32 | y) ^ prevXor;
            prevXor = plain;
            BinaryPrimitives.WriteUInt64BigEndian(result.AsSpan(i), plainXor);
        }

        return result;
    }

    public static byte[] Decrypt(Span<byte> data, Span<byte> key)
    {
        uint a = BinaryPrimitives.ReadUInt32BigEndian(key[..]);
        uint b = BinaryPrimitives.ReadUInt32BigEndian(key[4..]);
        uint c = BinaryPrimitives.ReadUInt32BigEndian(key[8..]);
        uint d = BinaryPrimitives.ReadUInt32BigEndian(key[12..]);

        var dest = new byte[data.Length];

        ulong plainXor = 0, prevXor = 0;
        for (int i = 0; i < data.Length; i += 8)
        {
            ulong plain = BinaryPrimitives.ReadUInt64BigEndian(data[i..]);
            plainXor ^= plain;
            uint x = (uint)(plainXor >> 32);
            uint y = (uint)(plainXor);

            y -= (x + 0xe3779b90) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0xe3779b90) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x454021d7) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x454021d7) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0xa708a81e) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0xa708a81e) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x08d12e65) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x08d12e65) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x6a99b4ac) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x6a99b4ac) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0xcc623af3) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0xcc623af3) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x2e2ac13a) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x2e2ac13a) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x8ff34781) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x8ff34781) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0xf1bbcdc8) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0xf1bbcdc8) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x5384540f) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x5384540f) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0xb54cda56) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0xb54cda56) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x1715609d) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x1715609d) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x78dde6e4) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x78dde6e4) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0xdaa66d2b) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0xdaa66d2b) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x3c6ef372) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x3c6ef372) ^ ((y << 4) + a) ^ ((y >> 5) + b);
            y -= (x + 0x9e3779b9) ^ ((x << 4) + c) ^ ((x >> 5) + d);
            x -= (y + 0x9e3779b9) ^ ((y << 4) + a) ^ ((y >> 5) + b);

            plainXor = ((ulong)x << 32) | y;
            BinaryPrimitives.WriteUInt64BigEndian(dest.AsSpan(i), plainXor ^ prevXor);
            prevXor = plain;
        }

        return dest[((dest[0] & 7) + 3)..^7];
    }
}