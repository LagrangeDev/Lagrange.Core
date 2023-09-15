using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class SendLongMsgResp
{
    [ProtoMember(2)] public SendLongMsgResult Result { get; set; }
    
    [ProtoMember(15)] public LongMsgSettings Settings { get; set; }
}

[ProtoContract]
internal class SendLongMsgResult
{
    [ProtoMember(3)] public string ResId { get; set; }
}
