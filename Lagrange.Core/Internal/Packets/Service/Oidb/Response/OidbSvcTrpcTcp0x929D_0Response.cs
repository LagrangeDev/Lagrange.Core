using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;
#pragma warning disable CS8618
[ProtoContract]
internal class OidbSvcTrpcTcp0x929D_0Response
{
    [ProtoMember(1)] public List<OidbSvcTrpcTcp0x929D_0ResponseKey> Property { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x929D_0ResponseKey
{
    [ProtoMember(1)] public string Type { get; set; }
    [ProtoMember(2)] public List<OidbSvcTrpcTcp0x929D_0ResponseProperty> Vulue { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x929D_0ResponseProperty
{
    [ProtoMember(1)] public string CharacterId { get; set; }

    [ProtoMember(2)] public string CharacterName { get; set; }

    [ProtoMember(3)] public string CharacterVoiceUrl { get; set; }
}