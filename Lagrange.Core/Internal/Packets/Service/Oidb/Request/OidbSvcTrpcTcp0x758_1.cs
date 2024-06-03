using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// Resharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x758, 1)]
internal class OidbSvcTrpcTcp0x758_1
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public List<OidbSvcTrpcTcp0x758_1Uid>? UidList { get; set; }
    
    [ProtoMember(10)] public uint? Field10 { get; set; } // 0
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x758_1Uid
{
    [ProtoMember(1)] public string? InviteUid { get; set; }
    
    [ProtoMember(2)] public uint? SourceGroupUin { get; set; }
}