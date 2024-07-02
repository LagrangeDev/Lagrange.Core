using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;
#pragma warning disable CS8618

[ProtoContract]
internal class GroupEssenceMessage
{
    // [ProtoMember(4)] public uint GroupUin; 有两个4

    [ProtoMember(13)] public uint Field13;

    [ProtoMember(33)] public EssenceMessage EssenceMessage;

    [ProtoMember(37)] public uint MsgSequence;

    [ProtoMember(39)] public uint Field39;
}

[ProtoContract]
internal class EssenceMessage
{
    [ProtoMember(1)] public uint GroupUin;

    [ProtoMember(2)] public uint MsgSequence;

    [ProtoMember(3)] public uint Random;

    [ProtoMember(4)] public uint SetFlag; // set 1  remove 2

    [ProtoMember(5)] public uint MemberUin;

    [ProtoMember(6)] public uint OperatorUin;

    [ProtoMember(7)] public uint TimeStamp;

    [ProtoMember(8)] public uint MsgSequence2; // removed 0

    [ProtoMember(9)] public string OperatorNickName;

    [ProtoMember(10)] public string MemberNickName;

    [ProtoMember(11)] public uint SetFlag2;
}