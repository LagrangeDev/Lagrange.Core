using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Get Cookie
/// </summary>
[ProtoContract]
internal class OidbSvcTrpcTcp0x88D
{
    [ProtoMember(1)]
    public uint Field1 { get; set; }

    [ProtoMember(2)]
    public OidbSvcTrpcTcp0x88DConfig Config { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x88DConfig
{
    [ProtoMember(1)]
    public ulong Uin { get; set; }

    [ProtoMember(2)]
    public OidbSvcTrpcTcp0x88DFlags Flags { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x88DFlags
{
    [ProtoMember(1)]
    public bool? OwnerUid { get; set; }

    [ProtoMember(2)]
    public bool? CreateTime { get; set; }

    [ProtoMember(5)]
    public bool? MaxMemberCount { get; set; }

    [ProtoMember(6)]
    public bool? MemberCount { get; set; }

    [ProtoMember(10)]
    public bool? Level { get; set; }

    [ProtoMember(15)]
    public string? Name { get; set; }

    [ProtoMember(16)]
    public string? NoticePreview { get; set; }

    [ProtoMember(21)]
    public bool? Uin { get; set; }

    [ProtoMember(22)]
    public bool? LastSequence { get; set; }

    [ProtoMember(23)]
    public bool? LastMessageTime { get; set; }

    [ProtoMember(24)]
    public bool? Question { get; set; }

    [ProtoMember(25)]
    public string? Answer { get; set; }

    [ProtoMember(29)]
    public string? MaxAdminCount { get; set; }
}