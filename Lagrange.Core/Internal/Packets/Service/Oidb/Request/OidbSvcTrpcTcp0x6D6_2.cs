using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group File Download
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6d6, 2)]
internal class OidbSvcTrpcTcp0x6D6_2
{
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D6_2Download? Download { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6_2Download
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
    
    [ProtoMember(3)] public uint BusId { get; set; } // 102
    
    [ProtoMember(4)] public string FileId { get; set; }
}