using System.Runtime.CompilerServices;

namespace Lagrange.Core.Utility.Generator;

internal static class ByteGen
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] GenRandomBytes(int length)
    {
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);
        return bytes;
    }
}