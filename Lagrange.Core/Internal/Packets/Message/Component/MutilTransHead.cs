using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component;

[ProtoContract]
internal class MutilTransHead
{
    [ProtoMember(1)] public int Status { get; set; }
    
    [ProtoMember(2)] public int MsgId { get; set; }
}