using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming

/// <summary>
/// Group File Space
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6d8, 3)]
internal class OidbSvcTrpcTcp0x6D8_3
{
    [ProtoMember(4)] public OidbSvcTrpcTcp0x6D8_1Space? Space { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1Space
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
}