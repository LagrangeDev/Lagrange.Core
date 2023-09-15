using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Action.Pb;

[ProtoContract]
internal class PbMultiMsgTransmit
{
    [ProtoMember(1)] public List<Message.Message> Msg { get; set; }
    
    [ProtoMember(2)] public List<PbMultiMsgItem> PbItemList { get; set; }
}