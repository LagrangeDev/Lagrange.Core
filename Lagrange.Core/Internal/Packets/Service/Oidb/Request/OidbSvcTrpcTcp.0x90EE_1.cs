using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0x90EE, 1)]
internal class OidbSvcTrpcTcp0x90EE_1
{
    [ProtoMember(1)] public uint FaceId { get; set; } 
    
    [ProtoMember(2)] public uint TargetMsgSeq { get; set; }
    
    [ProtoMember(3)] public uint TargetMsgSeq_2 { get; set; }
    
    [ProtoMember(4)] public int Field4 { get; set; } // group 2 friend 1 ?
    
    [ProtoMember(5)] public uint? TargetGroupId { get; set; }
    
    [ProtoMember(6)] public string? TargetUid { get; set; }
    
}