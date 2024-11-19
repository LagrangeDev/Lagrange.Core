using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0x5d6, 1)]
internal class OidbSvcTrpcTcp0x5D6_1
{
    [ProtoMember(1)] public uint Field1 { get; set; }

    [ProtoMember(2)] public OidbSvcTrpcTcp0x5D6_1Info Info { get; set; }

    [ProtoMember(3)] public uint Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x5D6_1Info
{
    [ProtoMember(2)] public uint GroupUin { get; set; }

    [ProtoMember(400)] public OidbSvcTrpcTcp0x5D6_1Field4_2_400 Field400 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x5D6_1Field4_2_400
{
    [ProtoMember(1)] public uint Field1 { get; set; }

    [ProtoMember(2)] public byte[] Timestamp { get; set; }
}