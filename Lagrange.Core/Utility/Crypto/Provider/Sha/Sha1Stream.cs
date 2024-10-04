using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lagrange.Core.Utility.Crypto.Provider.Sha;

public class Sha1Stream
{
    private readonly uint[] _state = new uint[5];
    private readonly int[] _count = new int[2];
    private readonly byte[] _buffer = new byte[Sha1BlockSize];

    public const int Sha1BlockSize = 64;
    public const int Sha1DigestSize = 20;

    private static readonly byte[] Padding =  // Constant array for padding
    {
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    public Sha1Stream() // Initialize SHA1 context
    {
        Reset();
    }

    private void Reset()
    {
        _state[0] = 0x67452301;
        _state[1] = 0xEFCDAB89;
        _state[2] = 0x98BADCFE;
        _state[3] = 0x10325476;
        _state[4] = 0xC3D2E1F0;
            
        _count[0] = 0;
        _count[1] = 0;
    }
    
    private unsafe void Transform(byte* data)  // Transform function
    {
        uint* w = stackalloc uint[80]; // 1. Break chunk into sixteen 32-bit words
        for (int i = 0; i < 16; i++)
        {
            var span = MemoryMarshal.CreateReadOnlySpan(ref data[i * 4], 4);
            w[i] = BinaryPrimitives.ReadUInt32BigEndian(span);
        }
        
        for (int i = 16; i < 80; i++) // 2. Extend the sixteen 32-bit words into eighty 32-bit words
        {
            w[i] = (w[i - 3] ^ w[i - 8] ^ w[i - 14] ^ w[i - 16]) << 1 | (w[i - 3] ^ w[i - 8] ^ w[i - 14] ^ w[i - 16]) >> 31;
        }
        uint a = _state[0];  // 3. Initialize hash value for this chunk
        uint b = _state[1];
        uint c = _state[2];
        uint d = _state[3];
        uint e = _state[4];

        for (int i = 0; i < 80; i++) // 4. Main loop
        {
            uint temp = i switch
            {
                < 20 => ((b & c) | (~b & d)) + 0x5A827999,
                < 40 => (b ^ c ^ d) + 0x6ED9EBA1,
                < 60 => ((b & c) | (b & d) | (c & d)) + 0x8F1BBCDC,
                _ => (b ^ c ^ d) + 0xCA62C1D6
            };

            temp += ((a << 5) | (a >> 27)) + e + w[i];
            e = d;
            d = c;
            c = (b << 30) | (b >> 2);
            b = a;
            a = temp;
        }

        _state[0] += a; // 5. Add this chunk's hash to result so far
        _state[1] += b;
        _state[2] += c;
        _state[3] += d;
        _state[4] += e;
    }
    
    public unsafe void Update(byte[] data, int len)  // Update SHA1 context
    {
        fixed (byte* ptr = data) UpdateInternal(ptr, len);
    }
    
    public unsafe void Update(Span<byte> data)  // Update SHA1 context
    {
        fixed (byte* ptr = data) UpdateInternal(ptr, data.Length);
    }
    
    private unsafe void UpdateInternal(byte* data, int len)
    {
        int index = (_count[0] >> 3) & 0x3F;
        _count[0] += len << 3;

        if (_count[0] < (len << 3)) _count[1]++;

        _count[1] += len >> 29;

        int partLen = Sha1BlockSize - index;
        int i = 0;

        if (len >= partLen)
        {
            fixed (byte* buffer = _buffer)
            {
                Unsafe.CopyBlock(buffer + index, data, (uint)partLen);
                Transform(buffer);
            }

            for (i = partLen; i + Sha1BlockSize <= len; i += Sha1BlockSize)
            {
                Transform(data + i);
            }

            index = 0;
        }

        fixed (byte* buffer = _buffer)
        {
            Unsafe.CopyBlock(buffer + index, data + i, (uint)(len - i));
        }
    }

    public void Hash(byte[] digest, bool bigEnding)
    {
        if (bigEnding)
        {
            for (int i = 0; i < _state.Length; i++)
            {
                BinaryPrimitives.WriteUInt32BigEndian(digest.AsSpan(i * 4), _state[i]);
            }
        }
        else
        {
            for (int i = 0; i < _state.Length; i++)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(digest.AsSpan(i * 4), _state[i]);
            }
        }
    }

    public void Final(byte[] digest)
    {
        if (digest.Length != Sha1DigestSize) throw new ArgumentException($"Digest array must be of size {Sha1DigestSize}");

        var bits = new byte[8];
        for (int i = 0; i < 8; i++)
        {
            int byteIndex = i >= 4 ? 0 : 1;
            int shift = (3 - (i & 3)) * 8;
            bits[i] = (byte)(_count[byteIndex] >> shift);
        }

        int index = (_count[0] >> 3) & 0x3F;
        int padLen = index < 56 ? 56 - index : 120 - index;

        Update(Padding, padLen);
        Update(bits, 8);

        for (int i = 0; i < Sha1DigestSize; i++)
        {
            int byteIndex = i >> 2;
            int shift = (3 - (i & 3)) * 8;
            digest[i] = (byte)(_state[byteIndex] >> shift);
        }
    }
}