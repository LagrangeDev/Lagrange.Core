using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming


/// <summary>
/// Group File Count
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6d8, 2)]
internal class OidbSvcTrpcTcp0x6D8_2
{
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D8_1Count? Count { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1Count
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
    
    [ProtoMember(3)] public uint BusId { get; set; } // 6
}