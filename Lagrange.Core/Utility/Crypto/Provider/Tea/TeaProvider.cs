using System.Runtime.CompilerServices;

namespace Lagrange.Core.Utility.Crypto.Provider.Tea;

internal static class TeaProvider
{
    private const uint Delta = 0x9E3779B9; // 0x9E3779B9, Fibonacci's hashing constant.

    private const long SumMax = (Delta << 4) & uint.MaxValue; // 0x9E3779B9 * 16 = 0x3C6EF35F, max sum value.

    public static byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key)
    {
        var keyStruct = new Key(key);
        int inputLength = data.Length;
        int fill = ((8 - ((inputLength + 10) & 7)) & 7) + 2;
        int length = fill + inputLength + 8;
        var cipher = new byte[length];
        cipher[0] = (byte)(RandomByte(248) | (fill - 2));

        for (int i = 1; i <= fill; ++i)
        {
            cipher[i] = RandomByte();
        }

        // Copy input data to the cipher buffer
        data.CopyTo(cipher.AsSpan(fill + 1));

        Span<byte> plainXorPrev = stackalloc byte[8];
        Span<byte> tempCipher = stackalloc byte[8];
        Span<byte> plainXor = stackalloc byte[8];
        Span<byte> encipher = stackalloc byte[8];

        for (int i = 0; i < length; i += 8)
        {
            // XOR current block with the previous cipher block
            for (int j = 0; j < 8; j++)
            {
                plainXor[j] = (byte)(cipher[i + j] ^ tempCipher[j]);
            }

            // Perform TEA encryption
            uint y = ReadUInt32(plainXor, 0);
            uint z = ReadUInt32(plainXor, 4);
            uint sum = 0;

            for (int j = 0; j < 16; ++j)
            {
                sum += Delta;
                y += ((z << 4) + keyStruct.A) ^ (z + sum) ^ ((z >> 5) + keyStruct.B);
                z += ((y << 4) + keyStruct.C) ^ (y + sum) ^ ((y >> 5) + keyStruct.D);
            }

            WriteUInt32(encipher, 0, y);
            WriteUInt32(encipher, 4, z);

            // XOR the encrypted block with the previous plain block
            for (int j = 0; j < 8; j++)
            {
                tempCipher[j] = (byte)(encipher[j] ^ plainXorPrev[j]);
            }

            // Copy the current plain block to plainXorPrev
            plainXor.CopyTo(plainXorPrev);

            // Write the encrypted block back to the cipher array
            tempCipher.CopyTo(cipher.AsSpan(i, 8));
        }

        return cipher;
    }
    public static byte[] Decrypt(Span<byte> data, Span<byte> key)
    {
        var keyStruct = new Key(key);
        int length = data.Length;
        if ((length & 7) != 0 || (length >> 4) == 0)
        {
            throw new Exception("Invalid cipher data length.");
        }

        var plain = new byte[length];
        var plainSub = new byte[8];
        var plainXor = new byte[8];

        for (int i = 0; i < length; i += 8)
        {
            for (int j = 0; j < 8; j++)
            {
                plainXor[j] = (byte)(data[i + j] ^ plainSub[j]);
            }

            long sum = SumMax;
            uint y = ReadUInt32(plainXor, 0);
            uint z = ReadUInt32(plainXor, 4);

            for (int j = 0; j < 16; ++j)
            {
                z -= (uint)(((y << 4) + keyStruct.C) ^ (y + sum) ^ ((y >> 5) + keyStruct.D));
                y -= (uint)(((z << 4) + keyStruct.A) ^ (z + sum) ^ ((z >> 5) + keyStruct.B));
                sum -= Delta;
            }

            WriteUInt32(plainSub, 0, y);
            WriteUInt32(plainSub, 4, z);

            for (int j = 0; j < 8; j++)
            {
                plain[i + j] = (byte)(plainSub[j] ^ (i >= 8 ? data[i + j - 8] : 0));
            }
        }

        for (int i = length - 7; i < length; ++i)
        {
            if (plain[i] != 0)
            {
                throw new Exception("Verification failed.");
            }
        }

        int from = (plain[0] & 7) + 3;
        return plain[from..(length - 7)];
    }

    private readonly struct Key
    {
        public uint A { get; }
        public uint B { get; }
        public uint C { get; }
        public uint D { get; }

        public Key(ReadOnlySpan<byte> rawKey)
        {
            if (rawKey.Length < 16)
            {
                throw new ArgumentException("Key must be at least 16 bytes long.", nameof(rawKey));
            }

            A = ReadUInt32(rawKey, 0);
            B = ReadUInt32(rawKey, 4);
            C = ReadUInt32(rawKey, 8);
            D = ReadUInt32(rawKey, 12);
        }
    }
    private static uint ReadUInt32(ReadOnlySpan<byte> data, int offset)
    {
        return (uint)((data[offset] << 24) |
                      (data[offset + 1] << 16) |
                      (data[offset + 2] << 8) |
                      data[offset + 3]);
    }

    private static void WriteUInt32(Span<byte> data, int offset, uint value)
    {
        data[offset] = (byte)((value >> 24) & 0xFF);
        data[offset + 1] = (byte)((value >> 16) & 0xFF);
        data[offset + 2] = (byte)((value >> 8) & 0xFF);
        data[offset + 3] = (byte)(value & 0xFF);
    }

    private static byte RandomByte(int max = byte.MaxValue) => (byte)(Random.Shared.Next() & max);
}
