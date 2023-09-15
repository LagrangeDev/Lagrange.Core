using Lagrange.Core.Internal.Packets.Message;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Action;

[ProtoContract]
internal class SendMessageRequest
{
    [ProtoMember(1)] public int State { get; set; }
    
    [ProtoMember(2)] public int SizeCache { get; set; }
    
    [ProtoMember(3)] public byte[] UnknownFields { get; set; }
    
    [ProtoMember(4)] public RoutingHead RoutingHead { get; set; }
    
    [ProtoMember(5)] public ContentHead ContentHead { get; set; }
    
    [ProtoMember(6)] public MessageBody MessageBody { get; set; }
    
    [ProtoMember(7)] public int MsgSeq { get; set; }
    
    [ProtoMember(8)] public int MsgRand { get; set; }
    
    [ProtoMember(9)] public byte[] SyncCookie { get; set; }
    
    [ProtoMember(10)] public int MsgVia { get; set; }
    
    [ProtoMember(11)] public int DataStatist { get; set; }
    
    [ProtoMember(12)] public MessageControl MessageControl { get; set; }
    
    [ProtoMember(13)] public int MultiSendSeq { get; set; }
}