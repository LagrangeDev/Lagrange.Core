using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupEssenceMessage
{
    [ProtoMember(1)] public uint GroupUin { get; set; }

    [ProtoMember(2)] public uint SourceSequence { get; set; }

    [ProtoMember(3)] public uint Field3 { get; set; }

    [ProtoMember(4)] public uint Type { get; set; }

    [ProtoMember(5)] public uint SenderUin { get; set; }

    [ProtoMember(6)] public uint OperatorUin { get; set; }

    [ProtoMember(7)] public uint OperateTime { get; set; }

    [ProtoMember(8)] public uint Sequence { get; set; } // 0 when unset

    [ProtoMember(9)] public string OperatorNick { get; set; }

    [ProtoMember(10)] public byte[] Field10 { get; set; } // 0x656D6D6D63

    // [ProtoMember(11)] public uint Type { get; set; }
}
