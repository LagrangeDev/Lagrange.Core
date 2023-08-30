using System.Runtime.CompilerServices;

namespace Lagrange.Core.Utility.Crypto.Provider.Tea;

internal static unsafe class TeaProvider
{
    private const long Delta = 2654435769L; // 0x9E3779B9, Fibonacci's hashing constant.
    
    private const long SumMax = (Delta << 4) & uint.MaxValue; // 0x9E3779B9 * 16 = 0x3C6EF35F, max sum value.

    public static byte[] Encrypt(Span<byte> data, Span<byte> key)
    {
        var keyStruct = new Key(key);
        int inputLength = data.Length;
        int fill = ((8 - ((inputLength + 10) & 7)) & 7) + 2;
        int length = fill + inputLength + 8;
        
        var cipher = new byte[length];
        cipher[0] = (byte)(RandomByte(248) | (fill - 2));
        for (int i = 1; i <= fill; ++i) cipher[i] = RandomByte();
        
        fixed (byte* dataPtr = cipher, rawPtr = data)
        {
            Buffer.MemoryCopy(rawPtr, dataPtr + fill + 1, inputLength, inputLength);

            byte* plainXorPrev = stackalloc byte[8];
            byte* tempCipher = stackalloc byte[8];
            byte* plainXor = stackalloc byte[8];
            byte* encipher = stackalloc byte[8];

            for (int i = 0; i < length; i += 8)
            {
                *(long*)plainXor = *(long*)(dataPtr + i) ^ *(long*)(tempCipher);

                long sum = 0;
                long y = ReadUInt32(plainXor, 0);
                long z = ReadUInt32(plainXor, 4);
                for (int j = 0; j < 16; ++j)
                {
                    sum += Delta;
                    sum &= uint.MaxValue;
                    y += ((z << 4) + keyStruct.A) ^ (z + sum) ^ ((z >> 5) + keyStruct.B);
                    y &= uint.MaxValue;
                    z += ((y << 4) + keyStruct.C) ^ (y + sum) ^ ((y >> 5) + keyStruct.D);
                    z &= uint.MaxValue;
                }
                
                WriteUInt32(encipher, 0, (uint)y);
                WriteUInt32(encipher, 4, (uint)z);
                
                *(long*)tempCipher = *(long*)(encipher) ^ *(long*)(plainXorPrev); // Xor8(EnCipher(plainXor, key), plainXorPrev);
                *(long*)(plainXorPrev) = *(long*)(plainXor); // write data back to plainXorPrev
                
                *(long*)(dataPtr + i) = *(long*)(tempCipher); // write data back to cipher
            }
        }
        
        return cipher;
    }

    public static byte[] Decrypt(Span<byte> data, Span<byte> key)
    {
        var keyStruct = new Key(key);
        int length = data.Length;
        if ((length & 7) != 0 || (length >> 4) == 0) throw new Exception("Invalid cipher data length.");
        
        var plain = new byte[length];
        byte* plainSub = stackalloc byte[8];
        byte* plainXor = stackalloc byte[8];

        fixed (byte* rawPtr = data, dataPtr = plain)
        {
            for (int i = 0; i < length; i += 8) // Decrypt data.
            {
                *(long*)plainXor = *(long*)(rawPtr + i) ^ *(long*)(plainSub);
                
                long sum = SumMax;
                long y = ReadUInt32(plainXor, 0);
                long z = ReadUInt32(plainXor, 4);
                for (int j = 0; j < 16; ++j)
                {
                    z -= ((y << 4) + keyStruct.C) ^ (y + sum) ^ ((y >> 5) + keyStruct.D);
                    z &= uint.MaxValue;
                    y -= ((z << 4) + keyStruct.A) ^ (z + sum) ^ ((z >> 5) + keyStruct.B);
                    y &= uint.MaxValue;
                    sum -= Delta;
                    sum &= uint.MaxValue;
                }
                
                WriteUInt32(plainSub, 0, (uint)y);
                WriteUInt32(plainSub, 4, (uint)z);
                
                *(long*)(dataPtr + i) = *(long*)(plainSub) ^ *(long*)(rawPtr + i - 8);
            }
            
            for (int i = length - 7; i < length; ++i) // Verify that the last 7 bytes are 0.
            {
                if (plain[i] != 0) throw new Exception("Verification failed.");
            }
            int from = (plain[0] & 7) + 3;  // Extract valid data.
            return plain[from..(length - 7)];
        }
    }

    private readonly struct Key
    {
        public uint A { get; }
        public uint B { get; }
        public uint C { get; }
        public uint D { get; }

        public Key(Span<byte> rawKey)
        {
            fixed (byte* rawKeyPtr = rawKey)
            {
                A = ReadUInt32(rawKeyPtr, 0);
                B = ReadUInt32(rawKeyPtr, 4);
                C = ReadUInt32(rawKeyPtr, 8);
                D = ReadUInt32(rawKeyPtr, 12);
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ReadUInt32(byte* data, int index) => (uint)(data[index] << 24 | data[index + 1] << 16 | data[index + 2] << 8 | data[index + 3]);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteUInt32(byte* data, int index, uint value)
    {
        data[index] = (byte)(value >> 24);
        data[index + 1] = (byte)(value >> 16);
        data[index + 2] = (byte)(value >> 8);
        data[index + 3] = (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte RandomByte(int max = byte.MaxValue) => (byte) (Random.Shared.Next() & max);
}