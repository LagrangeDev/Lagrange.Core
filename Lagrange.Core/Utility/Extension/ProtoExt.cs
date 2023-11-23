using System.Runtime.CompilerServices;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Utility.Extension;

internal static class ProtoExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BinaryPacket Serialize<T>(this T payload)
    {
        var stream = new MemoryStream();
        Serializer.Serialize(stream, payload);
        return new BinaryPacket(stream);
    }
}