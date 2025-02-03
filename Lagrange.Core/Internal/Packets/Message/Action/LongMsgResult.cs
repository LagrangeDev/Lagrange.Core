using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Action;

[ProtoContract]
internal class LongMsgResult
{
    [ProtoMember(2)] public List<LongMsgAction> Action { get; set; }
}

[ProtoContract]
internal class LongMsgAction
{
    [ProtoMember(1)] public string ActionCommand { get; set; }
    
    [ProtoMember(2)] public LongMsgContent ActionData { get; set; }
}

[ProtoContract]
internal class LongMsgContent
{
    [ProtoMember(1)] public List<PushMsgBody> MsgBody { get; set; }
}