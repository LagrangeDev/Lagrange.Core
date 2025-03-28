using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
public class StreamExtra
{
    [ProtoMember(43)] public ulong Field43 { get; init; }

    [ProtoMember(103)] public ulong Sequence { get; init; }
}