using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Highway;

[ProtoContract]
internal class ReqDataHighwayHead
{
    [ProtoMember(1)] public DataHighwayHead? MsgBaseHead { get; set; }
    
    [ProtoMember(2)] public SegHead? MsgSegHead { get; set; }
    
    [ProtoMember(3)] public byte[]? BytesReqExtendInfo { get; set; }
    
    [ProtoMember(4)] public ulong Timestamp { get; set; }
    
    [ProtoMember(5)] public LoginSigHead? MsgLoginSigHead { get; set; }
}