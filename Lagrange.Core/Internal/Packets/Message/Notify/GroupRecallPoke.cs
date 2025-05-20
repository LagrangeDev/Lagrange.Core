using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
public class GroupRecallPoke
{
    [ProtoMember(1)] public string OperatorUid { get; set; }

    [ProtoMember(3)] public uint GroupUin { get; set; }

    [ProtoMember(4)] public ulong BusiId { get; set; }

    [ProtoMember(5)] public ulong TipsSeqId { get; set; }
}
