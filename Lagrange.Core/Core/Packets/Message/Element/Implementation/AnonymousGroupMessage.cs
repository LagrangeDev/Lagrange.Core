using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class AnonymousGroupMessage
{
    [ProtoMember(1)] public int Flags { get; set; }
    
    [ProtoMember(2)] public byte[] AnonId { get; set; }
    
    [ProtoMember(3)] public byte[] AnonNick { get; set; }
    
    [ProtoMember(4)] public int HeadPortrait { get; set; }
    
    [ProtoMember(5)] public int ExpireTime { get; set; }
    
    [ProtoMember(6)] public int BubbleId { get; set; }
    
    [ProtoMember(7)] public byte[] RankColor { get; set; }
}