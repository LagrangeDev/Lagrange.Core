using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message;

[ProtoContract]
internal class MessageControl
{
    [ProtoMember(1)] public int MsgFlag { get; set; }
}