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

[ProtoContract]
public class Business
{
    [ProtoMember(3)] public BusinessBody? Body { get; set; }
}

[ProtoContract]
public class BusinessBody
{
    [ProtoMember(1)] public string? Msg { get; set; }

    [ProtoMember(3)] public List<BusinessList>? Lists { get; set; }
}

[ProtoContract]
public class BusinessList
{
    [ProtoMember(1)] public uint Type { get; set; }

    [ProtoMember(2)] internal uint Field2 { get; set; }

    [ProtoMember(3)] public uint IsYear { get; set; } // 是否年费

    [ProtoMember(4)] public uint Level { get; set; }

    [ProtoMember(5)] public uint IsPro { get; set; } // 是否为超级

    [ProtoMember(6)] internal string? Icon1 { get; set; }

    [ProtoMember(7)] internal string? Icon2 { get; set; }

    public string? Icon => !string.IsNullOrEmpty(Icon1) ? Icon1 : Icon2;

}