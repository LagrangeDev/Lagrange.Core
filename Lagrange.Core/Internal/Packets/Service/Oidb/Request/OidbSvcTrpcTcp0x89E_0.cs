using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// Resharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x89e, 0)]
internal class OidbSvcTrpcTcp0x89E_0
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public string SourceUid { get; set; }
    
    [ProtoMember(3)] public string TargetUid { get; set; }
}