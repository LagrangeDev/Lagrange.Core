using System.Runtime.CompilerServices;
using ProtoBuf;

namespace Lagrange.Core.Utility.Extension;

internal static class ProtoExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> Serialize<T>(this T payload)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, payload);
        return stream.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] SerializeToBytes<T>(this T payload)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, payload);
        return stream.ToArray();
    }
}