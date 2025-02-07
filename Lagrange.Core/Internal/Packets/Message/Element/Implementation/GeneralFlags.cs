using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class GeneralFlags
{
    [ProtoMember(1)] public uint BubbleDiyTextId { get; set; }
    
    [ProtoMember(2)] public int GroupFlagNew { get; set; }
    
    [ProtoMember(3)] public ulong Uin { get; set; }
    
    [ProtoMember(4)] public byte[] RpId { get; set; }
    
    [ProtoMember(5)] public int PrpFold { get; set; }
    
    [ProtoMember(6)] public int LongTextFlag { get; set; }
    
    [ProtoMember(7)] public string? LongTextResId { get; set; }
    
    [ProtoMember(8)] public int GroupType { get; set; }
    
    [ProtoMember(9)] public int ToUinFlag { get; set; }
    
    [ProtoMember(10)] public int GlamourLevel { get; set; }
    
    [ProtoMember(11)] public int MemberLevel { get; set; }
    
    [ProtoMember(12)] public long GroupRankSeq { get; set; }
    
    [ProtoMember(13)] public int OlympicTorch { get; set; }
    
    [ProtoMember(14)] public byte[] BabyqGuideMsgCookie { get; set; }
    
    [ProtoMember(15)] public int Uin32ExpertFlag { get; set; }
    
    [ProtoMember(16)] public int BubbleSubId { get; set; }
    
    [ProtoMember(17)] public ulong PendantId { get; set; }
    
    [ProtoMember(18)] public byte[] RpIndex { get; set; }
    
    [ProtoMember(19)] public byte[] PbReserve { get; set; }
}