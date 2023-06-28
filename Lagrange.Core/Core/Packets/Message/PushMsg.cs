using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message;

#pragma warning disable CS8618

[ProtoContract]
internal class PushMsg
{
    [ProtoMember(1)] public PushMsgBody Message { get; set; }
}

[ProtoContract]
internal class PushMsgBody
{
    [ProtoMember(1)] public ResponseHead ResponseHead { get; set; }
    
    [ProtoMember(2)] public ContentHead ContentHead { get; set; }
    
    [ProtoMember(3)] public MessageBody Body { get; set; }
}