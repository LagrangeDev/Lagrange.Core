using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
internal class OidbSvcTrpcTcp0x12B3_0Response
{
    [ProtoMember(1)] public List<OidbSvcTrpcTcp0x12B3_0ResponseFriend>? Friends { get; set; }

    [ProtoMember(3)] public List<OidbSvcTrpcTcp0x12B3_0ResponseGroup>? Groups { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x12B3_0ResponseFriend
{
    [ProtoMember(1)] public string Uid { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x12B3_0ResponseGroup
{
    [ProtoMember(1)] public uint Uin { get; set; }
}