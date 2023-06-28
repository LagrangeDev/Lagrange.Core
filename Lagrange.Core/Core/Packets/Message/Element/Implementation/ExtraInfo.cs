using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class ExtraInfo
{
    [ProtoMember(1)] public byte[] Nick { get; set; }
    
    [ProtoMember(2)] public byte[] GroupCard { get; set; }
    
    [ProtoMember(3)] public int Level { get; set; }
    
    [ProtoMember(4)] public int Flags { get; set; }
    
    [ProtoMember(5)] public int GroupMask { get; set; }
    
    [ProtoMember(6)] public int MsgTailId { get; set; }
    
    [ProtoMember(7)] public byte[] SenderTitle { get; set; }
    
    [ProtoMember(8)] public byte[] ApnsTips { get; set; }
    
    [ProtoMember(9)] public ulong Uin { get; set; }
    
    [ProtoMember(10)] public int MsgStateFlag { get; set; }
    
    [ProtoMember(11)] public int ApnsSoundType { get; set; }
    
    [ProtoMember(12)] public int NewGroupFlag { get; set; }
}