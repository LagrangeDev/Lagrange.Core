using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x88D_0Response
{
    [ProtoMember(1)]
    public OidbSvcTrpcTcp0x88D_0ResponseGroupInfo GroupInfo { get; set; }
}

[ProtoContract]
public class OidbSvcTrpcTcp0x88D_0ResponseGroupInfo
{
    [ProtoMember(1)]
    public ulong Uin { get; set; }

    [ProtoMember(3)]
    public OidbSvcTrpcTcp0x88D_0ResponseResults Results { get; set; }
}

[ProtoContract]
public class OidbSvcTrpcTcp0x88D_0ResponseResults
{
    [ProtoMember(1)]
    public string OwnerUid { get; set; }

    [ProtoMember(2)]
    public ulong CreateTime { get; set; }

    [ProtoMember(5)]
    public ulong MaxMemberCount { get; set; }

    [ProtoMember(6)]
    public ulong MemberCount { get; set; }

    [ProtoMember(10)]
    public ulong Level { get; set; }

    [ProtoMember(15)]
    public string Name { get; set; }

    [ProtoMember(16)]
    public string NoticePreview { get; set; }

    [ProtoMember(21)]
    public ulong Uin { get; set; }

    [ProtoMember(22)]
    public ulong LastSequence { get; set; }

    [ProtoMember(23)]
    public ulong LastMessageTime { get; set; }

    [ProtoMember(24)]
    public string Question { get; set; }

    [ProtoMember(25)]
    public string Answer { get; set; }

    [ProtoMember(29)]
    public ulong MaxAdminCount { get; set; }
}