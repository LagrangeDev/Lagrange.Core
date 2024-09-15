using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Routing;

[ProtoContract]
internal class ResponseGrp
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(4)] public string MemberName { get; set; }
    
    [ProtoMember(5)] public uint Unknown5 { get; set; }
    
    [ProtoMember(7)] public string GroupName { get; set; }
}