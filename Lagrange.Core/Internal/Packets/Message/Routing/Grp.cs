using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Routing;

[ProtoContract]
internal class Grp
{
    [ProtoMember(1)] public uint? GroupCode { get; set; }
}