using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class GroupMemberEnter
{
    [ProtoMember(1)] public bool Field1 { get; set; }

    [ProtoMember(2)] public GroupMemberEnterContentBody Body { get; set; }
}

[ProtoContract]
internal class GroupMemberEnterContentBody
{
    [ProtoMember(1)] public GroupMemberEnterContentBodyField1 Field1 { get; set; }

    [ProtoMember(2)] public uint GroupId { get; set; }

    [ProtoMember(3)] public uint Field3 { get; set; }

    [ProtoMember(4)] public GroupMemberEnterInfo Info { get; set; }

    [ProtoMember(5)] public uint Field5 { get; set; }
}

[ProtoContract]
internal class GroupMemberEnterContentBodyField1
{
    [ProtoMember(1)] public uint Field1 { get; set; }
}

[ProtoContract]
internal class GroupMemberEnterInfo
{
    [ProtoMember(2)] public uint Field2 { get; set; }

    [ProtoMember(3)] public GroupMemberEnterDetail Detail { get; set; }

    [ProtoMember(4)] public uint Field4 { get; set; }

    [ProtoMember(5)] public uint Field5 { get; set; }
}

[ProtoContract]
internal class GroupMemberEnterDetail
{
    [ProtoMember(3)] public uint GroupMemberUin { get; set; }
    [ProtoMember(4)] public uint GroupId { get; set; }
    [ProtoMember(5)] public uint Field5 { get; set; }
    [ProtoMember(6)] public uint Field6 { get; set; }
    [ProtoMember(20)] public GroupMemberEnterStyle Style { get; set; }
    [ProtoMember(21)] public uint Field21 { get; set; }
}

[ProtoContract]
internal class GroupMemberEnterStyle
{
    [ProtoMember(1)] public uint StyleId { get; set; }
}