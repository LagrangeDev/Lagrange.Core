using ProtoBuf;
#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0xE37, 1200)]
internal class OidbSvcTrpcTcp0xE37_1200
{
    [ProtoMember(1)] public uint SubCommand { get; set; } = 1200;

    [ProtoMember(2)] public int Field2 { get; set; } = 1; // Unknown
    
    [ProtoMember(14)] public OidbSvcTrpcTcp0xE37_1200Body Body { get; set; }

    [ProtoMember(101)] public int Field101 { get; set; } = 3; // Unknown

    [ProtoMember(102)] public int Field102 { get; set; } = 103; // Unknown

    [ProtoMember(200)] public int Field200 { get; set; } = 1;  // Unknown

    [ProtoMember(99999)] public byte[] Field99999 { get; set; } = { 0xc0, 0x85, 0x2c, 0x01 }; // Actually it is a sub-proto 90200: 1, but we it would be more easy to just hardcode it
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37_1200Body
{
    [ProtoMember(10)] public string ReceiverUid { get; set; }
    
    [ProtoMember(20)] public string FileUuid { get; set; }

    [ProtoMember(30)] public int Type { get; set; } = 2;
    
    [ProtoMember(60)] public string FileHash { get; set; }

    [ProtoMember(601)] public int T2 { get; set; } = 0;
}