using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupRecall
{
    [ProtoMember(1)] public string? OperatorUid { get; set; }

    [ProtoMember(3)] public List<RecallMessage> RecallMessages { get; set; }

    [ProtoMember(5)] public byte[] UserDef { get; set; }

    [ProtoMember(6)] public int GroupType { get; set; }

    [ProtoMember(7)] public int OpType { get; set; }

    [ProtoMember(9)] public GroupRecallTipInfo? TipInfo { get; set; }
}

[ProtoContract]
internal class RecallMessage
{
    [ProtoMember(1)] public uint Sequence { get; set; }

    [ProtoMember(2)] public uint Time { get; set; }

    [ProtoMember(3)] public uint Random { get; set; }

    [ProtoMember(4)] public uint Type { get; set; }

    [ProtoMember(5)] public uint Flag { get; set; }

    [ProtoMember(6)] public string AuthorUid { get; set; }
}

[ProtoContract]
internal class GroupRecallTipInfo
{
    [ProtoMember(2)] public string Tip { get; set; }
}