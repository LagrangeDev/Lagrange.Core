using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group Member Mute
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x1253, 1)]
internal class OidbSvcTrpcTcp0x1253_1
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Type { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x1253_1Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x1253_1Body
{
    [ProtoMember(1)] public string TargetUid { get; set; }
    
    [ProtoMember(2)] public uint Duration { get; set; }
}