using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Rename Group Member
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x8FC, 3)]
internal class OidbSvcTrpcTcp0x8FC_3
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x8FC_3Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x8FC_3Body
{
    [ProtoMember(1)] public string TargetUid { get; set; }
    
    [ProtoMember(8)] public string TargetName { get; set; }
}