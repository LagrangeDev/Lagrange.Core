using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Action;

[ProtoContract]
internal class GetGroupMsgResp
{
    [ProtoMember(1)] public uint Result { get; set; }
    
    [ProtoMember(2)] public string ErrMsg { get; set; }
    
    [ProtoMember(3)] public ulong GroupCode { get; set; }
    
    [ProtoMember(4)] public ulong ReturnBeginSeq { get; set; }
    
    [ProtoMember(5)] public ulong ReturnEndSeq { get; set; }
    
    [ProtoMember(6)] public List<Message.Message> Msg { get; set; }
}