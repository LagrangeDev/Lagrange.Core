using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;
#pragma warning disable CS8618
[ProtoContract]
internal class OidbSvcTrpcTcp0x929C_0Response
{
    [ProtoMember(1)] public List<OidbSvcTrpcTcp0x929C_0ResponseProperty> Property { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x929C_0ResponseProperty
{
    [ProtoMember(1)] public string CharacterId { get; set; }

    [ProtoMember(2)] public string CharacterName { get; set; }

    [ProtoMember(3)] public string CharacterVoiceUrl { get; set; }
}