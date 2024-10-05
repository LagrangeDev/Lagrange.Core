using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Fetch Group Member List
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xfe7, 3)]
internal class OidbSvcTrpcTcp0xFE7_3
{
    [ProtoMember(1)] public uint GroupUin { get; set; }

    [ProtoMember(2)] public uint Field2 { get; set; } // 5

    [ProtoMember(3)] public uint Field3 { get; set; } // 2

    [ProtoMember(4)] public OidbSvcTrpcScp0xFE7_3Body Body { get; set; }

    [ProtoMember(15)] public string? Token { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcScp0xFE7_3Body
{
    [ProtoMember(10)] public bool MemberName { get; set; } = true; // 1

    [ProtoMember(11)] public bool MemberCard { get; set; } = true; // 1

    [ProtoMember(12)] public bool Level { get; set; } = true; // 1

    [ProtoMember(13)] public bool Field13 { get; set; } = true; // 1

    [ProtoMember(16)] public bool Field16 { get; set; } = true; // 1

    [ProtoMember(17)] public bool SpecialTitle { get; set; } = true; // 1

    [ProtoMember(18)] public bool Field18 { get; set; } = true; // 1

    [ProtoMember(20)] public bool Field20 { get; set; } = true; // 1

    [ProtoMember(21)] public bool Field21 { get; set; } = true; // 1

    [ProtoMember(100)] public bool JoinTimestamp { get; set; } = true; // 1

    [ProtoMember(101)] public bool LastMsgTimestamp { get; set; } = true; // 1

    [ProtoMember(102)] public bool ShutUpTimestamp { get; set; } = true; // 1

    [ProtoMember(103)] public bool Field103 { get; set; } = true; // 1

    [ProtoMember(104)] public bool Field104 { get; set; } = true; // 1

    [ProtoMember(105)] public bool Field105 { get; set; } = true; // 1

    [ProtoMember(106)] public bool Field106 { get; set; } = true; // 1

    [ProtoMember(107)] public bool Permission { get; set; } = true; // 1

    [ProtoMember(200)] public bool Field200 { get; set; } = true;

    [ProtoMember(201)] public bool Field201 { get; set; } = true;
}