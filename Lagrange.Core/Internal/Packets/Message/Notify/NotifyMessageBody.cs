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
    
    [ProtoMember(11)] public GroupRecall Recall { get; set; }
}