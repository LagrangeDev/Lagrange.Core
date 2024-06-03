using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

[ProtoContract]
internal class RespDataHighwayHead
{
    [ProtoMember(1)] public DataHighwayHead? MsgBaseHead { get; set; }
    
    [ProtoMember(2)] public SegHead? MsgSegHead { get; set; }
    
    [ProtoMember(3)] public uint ErrorCode { get; set; }
    
    [ProtoMember(4)] public uint AllowRetry { get; set; }
    
    [ProtoMember(5)] public uint CacheCost { get; set; }
    
    [ProtoMember(6)] public uint HtCost { get; set; }
    
    [ProtoMember(7)] public byte[]? BytesRspExtendInfo { get; set; }
    
    [ProtoMember(8)] public ulong Timestamp { get; set; }
    
    [ProtoMember(9)] public ulong Range { get; set; }
    
    [ProtoMember(10)] public uint IsReset { get; set; }
}