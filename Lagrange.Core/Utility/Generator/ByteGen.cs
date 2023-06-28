using System.Runtime.CompilerServices;

namespace Lagrange.Core.Utility.Generator;

internal static class ByteGen
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] GenRandomBytes(int length)
    {
        var bytes = new byte[length];
        for (int i = 0; i < length; ++i) bytes[i] = (byte) Random.Shared.Next(0, 256);
        return bytes;
    }
}