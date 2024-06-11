using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Lagrange.Core.Utility.Extension;

internal static class ByteExtension
{
    private interface IHexByteStruct
    {
        void Write(uint hexChar);
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 6)]
    private struct WithSpaceHexByteStruct : IHexByteStruct
    {
        [FieldOffset(0)] public uint twoChar;
        [FieldOffset(4)] public char space;

        public void Write(uint hexChar)
        {
            twoChar = hexChar;
            space = ' ';
        }
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 4)]
    private struct NoSpaceHexByteStruct : IHexByteStruct
    {
        [FieldOffset(0)] public uint twoChar;

        public void Write(uint hexChar)
            => twoChar = hexChar;
    }

    public static string Hex(this byte[] bytes, bool lower = false, bool space = false)
        => Hex(bytes.AsSpan(), lower, space);

    public static string Hex(this Span<byte> bytes, bool lower = true, bool space = false)
        => Hex((ReadOnlySpan<byte>)bytes, lower, space);

    public static string Hex(this ReadOnlySpan<byte> bytes, bool lower = true, bool space = false)
        => space ? HexInternal<WithSpaceHexByteStruct>(bytes, lower) : HexInternal<NoSpaceHexByteStruct>(bytes, lower);

    private static string HexInternal<TStruct>(ReadOnlySpan<byte> bytes, bool lower) where TStruct : struct, IHexByteStruct
    {
        if (bytes.Length == 0) return string.Empty;

        uint casing = lower ? 0x200020u : 0;
        int structSize = Marshal.SizeOf<TStruct>();
        if (structSize % 2 == 1) throw new ArgumentException($"{nameof(TStruct)}'s size of must be a multiple of 2, currently {structSize}");

        int charCountPerByte = structSize / 2;  // 2 is the size of char
        string result = new string('\0', bytes.Length * charCountPerByte);
        var resultSpan = MemoryMarshal.CreateSpan(ref Unsafe.As<char, TStruct>(ref Unsafe.AsRef(in result.GetPinnableReference())), bytes.Length);

        for (int i = 0; i < bytes.Length; i++) resultSpan[i].Write(ToCharsBuffer(bytes[i], casing));

        return result;
    }

    public static string Md5(this byte[] bytes, bool lower = false)
    {
        return MD5.HashData(bytes).Hex(lower);
    }
    
    public static async Task<string> Md5Async(this byte[] bytes, bool lower = false)
    {
        using var md5 = MD5.Create();
        using var stream = new MemoryStream(bytes);
        var hash = await md5.ComputeHashAsync(stream);
        return hash.Hex(lower);
    }

    // By Executor-Cheng https://github.com/KonataDev/Lagrange.Core/pull/344#pullrequestreview-2027515322
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ToCharsBuffer(byte value, uint casing = 0)
    {
        uint difference = BitConverter.IsLittleEndian 
            ? ((uint)value >> 4) + ((value & 0x0Fu) << 16) - 0x890089u 
            : ((value & 0xF0u) << 12) + (value & 0x0Fu) - 0x890089u;
        uint packedResult = ((((uint)-(int)difference & 0x700070u) >> 4) + difference + 0xB900B9u) | casing;
        return packedResult;
    }
}