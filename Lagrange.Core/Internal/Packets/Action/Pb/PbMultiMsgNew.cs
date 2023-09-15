using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Action.Pb;

[ProtoContract]
internal class PbMultiMsgNew
{
    [ProtoMember(1)] public List<Message.Message> Msg { get; set; }
}