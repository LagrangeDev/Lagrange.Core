using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Rename Group Title
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x89A, 15)]
internal class OidbSvcTrpcTcp0x89A_15
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x89A_15Body Body { get; set; }
}

internal class OidbSvcTrpcTcp0x89A_15Body
{
    [ProtoMember(3)] public string TargetName { get; set; }
}