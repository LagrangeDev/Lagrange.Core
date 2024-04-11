using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

/// <summary>
/// group0x857.proto MessageRecallReminder
/// </summary>
[ProtoContract]
internal class NotifyMessageBody
{
    [ProtoMember(1)] public uint Type { get; set; }
    
    [ProtoMember(4)] public uint GroupUin { get; set; }

    [ProtoMember(5)] public List<GroupUniqueTitleChange> UniqueTitleChange { get; set; }

    [ProtoMember(11)] public GroupRecall? Recall { get; set; }

    // [ProtoMember(13)] public uint Field13 { get; set; }

    [ProtoMember(26)] public List<GroupCommonTips> CommonTips { get; set; }

    [ProtoMember(33)] public List<GroupEssenceMessage> EssenceMessage { get; set; }

    // [ProtoMember(37)] public uint Sequence { get; set; }

    // [ProtoMember(39)] public uint Field39 { get; set; }

    [ProtoMember(44)] public List<GroupReactMessageWithEmoji> ReactEmoji { get; set; }
}