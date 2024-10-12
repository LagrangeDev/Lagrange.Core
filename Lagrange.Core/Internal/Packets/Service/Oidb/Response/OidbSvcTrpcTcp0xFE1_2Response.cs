using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using ProtoBuf;

#pragma warning disable CS8618
// Resharper Disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;


[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xFE1_2ResponseBody Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2ResponseBody
{
    // [ProtoMember(1)] public string Uid { get; set; } = "";

    [ProtoMember(2)] public OidbSvcTrpcTcp0xFE1_2ResponseProperty Properties { get; set; }

    [ProtoMember(3)] public uint Uin { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2ResponseProperty
{
    [ProtoMember(1)] public List<OidbTwoNumber> NumberProperties { get; set; }

    [ProtoMember(2)] public List<OidbFriendByteProperty> BytesProperties { get; set; }
}

[ProtoContract]
public class CustomStatus
{
    [ProtoMember(1)] public uint FaceId { get; set; }

    [ProtoMember(2)] public string? Msg { get; set; }
}

[ProtoContract]
public class Avatar
{
    [ProtoMember(5)] public string? Url { get; set; }
}