using Lagrange.Core.Internal.Packets.Message.Component;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal partial class ElemFlags2
{
    [ProtoMember(1)] public ulong ColorTextId { get; set; }
    
    [ProtoMember(2)] public ulong MsgId { get; set; }
    
    [ProtoMember(3)] public uint WhisperSessionId { get; set; }
    
    [ProtoMember(4)] public uint PttChangeBit { get; set; }
    
    [ProtoMember(5)] public uint VipStatus { get; set; }
    
    [ProtoMember(6)] public uint CompatibleId { get; set; }
    
    [ProtoMember(7)] public List<Instance> Insts { get; set; }
    
    [ProtoMember(8)] public uint MsgRptCnt { get; set; }
    
    [ProtoMember(9)] public Instance SrcInst { get; set; }
    
    [ProtoMember(10)] public uint Longtitude { get; set; }
    
    [ProtoMember(11)] public uint Latitude { get; set; }
    
    [ProtoMember(12)] public uint CustomFont { get; set; }
    
    [ProtoMember(13)] public PcSupportDef PcSupportDef { get; set; }
    
    [ProtoMember(14)] public uint? CrmFlags { get; set; }
}