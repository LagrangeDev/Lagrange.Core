using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group Remark
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xF16, 1)]
internal class OidbSvcTrpcTcp0xF16_1
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xF16_1Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xF16_1Body
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public string TargetRemark { get; set; }
}