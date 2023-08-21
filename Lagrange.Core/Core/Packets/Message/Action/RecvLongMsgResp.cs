using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Action;

[ProtoContract]
internal class RecvLongMsgResp
{
    [ProtoMember(1)] public RecvLongMsgResult Result { get; set; }
    
    [ProtoMember(15)] public LongMsgSettings Settings { get; set; }
}

[ProtoContract]
internal class RecvLongMsgResult
{
    [ProtoMember(3)] public string ResId { get; set; }
    
    [ProtoMember(4)] public byte[] Payload { get; set; }
}