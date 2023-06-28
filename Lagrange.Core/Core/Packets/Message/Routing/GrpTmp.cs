using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Routing;

[ProtoContract]
internal class GrpTmp
{
    [ProtoMember(1)] public uint? GroupUin { get; set; }
    
    [ProtoMember(2)] public uint? ToUin { get; set; }
}