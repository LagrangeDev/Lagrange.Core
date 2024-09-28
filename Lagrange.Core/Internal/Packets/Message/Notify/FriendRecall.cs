using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class FriendRecall
{
    [ProtoMember(1)] public FriendRecallInfo Info { get; set; }

    [ProtoMember(2)] public uint InstId { get; set; }

    [ProtoMember(3)] public uint AppId { get; set; }

    [ProtoMember(4)] public uint LongMessageFlag { get; set; }

    [ProtoMember(5)] public byte[] Reserved { get; set; }
}

[ProtoContract]
internal class FriendRecallInfo
{
    [ProtoMember(1)] public string FromUid { get; set; }

    [ProtoMember(2)] public string ToUid { get; set; }

    [ProtoMember(3)] public uint ClientSequence { get; set; }

    [ProtoMember(4)] public ulong NewId { get; set; }

    [ProtoMember(5)] public uint Time { get; set; }

    [ProtoMember(6)] public uint Random { get; set; }

    [ProtoMember(7)] public uint PkgNum { get; set; }

    [ProtoMember(8)] public uint PkgIndex { get; set; }

    [ProtoMember(9)] public uint DivSeq { get; set; }

    [ProtoMember(13)] public FriendRecallTipInfo TipInfo { get; set; }
}

[ProtoContract]
internal class FriendRecallTipInfo
{
    [ProtoMember(2)] public string? Tip { get; set; }
}