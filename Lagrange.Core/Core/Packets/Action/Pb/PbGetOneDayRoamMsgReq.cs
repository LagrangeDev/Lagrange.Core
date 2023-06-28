using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Action.Pb;

[ProtoContract]
internal class PbGetOneDayRoamMsgReq
{
    [ProtoMember(1)] public ulong PeerUin { get; set; }
    
    [ProtoMember(2)] public ulong LastMsgTime { get; set; }
    
    [ProtoMember(3)] public ulong Random { get; set; }
    
    [ProtoMember(4)] public uint ReadCnt { get; set; }
}