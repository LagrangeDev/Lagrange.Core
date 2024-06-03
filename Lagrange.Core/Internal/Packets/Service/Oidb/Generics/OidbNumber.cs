using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Generics;

[ProtoContract]
internal class OidbNumber
{
    [ProtoMember(1)] public List<uint> Numbers { get; set; } = new();
}