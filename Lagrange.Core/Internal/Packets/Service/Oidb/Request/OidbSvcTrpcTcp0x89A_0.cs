using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

/// <summary>
/// Group Global Mute
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x89A, 0)]
internal class OidbSvcTrpcTcp0x89A_0
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x89A_0State State { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x89A_0State
{
    [ProtoMember(17)] public uint S { get; set; }
}