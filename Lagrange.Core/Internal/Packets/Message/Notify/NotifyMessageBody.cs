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
    
    [ProtoMember(5)] public byte[]? EventParam { get; set; }
    
    [ProtoMember(11)] public GroupRecall Recall { get; set; }
    
    [ProtoMember(13)] public uint? Field13 { get; set; }
    
    [ProtoMember(21)] public string OperatorUid { get; set; }
    
    [ProtoMember(26)] public GeneralGrayTipInfo GeneralGrayTip { get; set; }

    [ProtoMember(33)] public EssenceMessage EssenceMessage;
    
    [ProtoMember(37)] public uint MsgSequence { get; set; }
    
    [ProtoMember(39)] public uint Field39 { get; set; }
    
    [ProtoMember(44)] public GroupReactionData0 Reaction { get; set; }
}

[ProtoContract]
internal class GeneralGrayTipInfo
{
    [ProtoMember(1)] public ulong BusiType { get; set; }
    
    [ProtoMember(2)] public ulong BusiId { get; set; }
    
    [ProtoMember(3)] public uint CtrlFlag { get; set; }
    
    [ProtoMember(4)] public uint C2CType { get; set; }
    
    [ProtoMember(5)] public uint ServiceType { get; set; }
    
    [ProtoMember(6)] public ulong TemplId { get; set; }
    
    [ProtoMember(7)] public TemplParam[] MsgTemplParam { get; set; }
    
    [ProtoMember(8)] public string Content { get; set; }
}

[ProtoContract]
internal class TemplParam
{
    [ProtoMember(1)] public string Name { get; set; }
    
    [ProtoMember(2)] public string Value { get; set; }
}