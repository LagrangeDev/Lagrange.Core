using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message;

/// <summary>
/// MessageSvc.PbSendMsg for Send, trpc.msg.olpush.OlPushService.MsgPush for Receive
/// </summary>
[ProtoContract]
internal class Message
{
    [ProtoMember(1)] public RoutingHead? RoutingHead { get; set; }
    
    [ProtoMember(2)] public ContentHead? ContentHead { get; set; }
    
    [ProtoMember(3)] public MessageBody? Body { get; set; }
    
    [ProtoMember(4)] public uint? ClientSequence { get; set; }
    
    [ProtoMember(5)] public uint? Random { get; set; }
    
    [ProtoMember(6)] public byte[]? SyncCookie { get; set; }
    
    // [ProtoMember(7)] public AppShareInfo? AppShare { get; set; }
    
    [ProtoMember(8)] public uint? Via { get; set; }
    
    [ProtoMember(9)] public uint? DataStatist { get; set; }
    
    // [ProtoMember(10)] public MultiMsgAssist? MultiMsgAssist { get; set; }
    
    // [ProtoMember(11)] public InputNotifyInfo? InputNotifyInfo { get; set; }
    
    [ProtoMember(12)] public MessageControl? Ctrl { get; set; }
    
    // [ProtoMember(13)] public ReceiptReq? ReceiptReq { get; set; }
    
    [ProtoMember(14)] public uint MultiSendSeq { get; set; }
}