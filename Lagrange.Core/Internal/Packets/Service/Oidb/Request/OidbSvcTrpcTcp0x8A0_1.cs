using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group Kick Member
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x8A0, 1)]
internal class OidbSvcTrpcTcp0x8A0_1
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public string TargetUid { get; set; }
    
    [ProtoMember(4)] public bool RejectAddRequest { get; set; }
    
    [ProtoMember(5)] public string Reason { get; set; }
}