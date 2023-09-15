using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

[ProtoContract]
internal class SyncCookie
{
    [ProtoMember(1)] public long Time1 { get; set; }
    
    [ProtoMember(2)] public long Time { get; set; }
    
    [ProtoMember(3)] public long Ran1 { get; set; }
    
    [ProtoMember(4)] public long Ran2 { get; set; }
    
    [ProtoMember(5)] public long Const1 { get; set; }
    
    [ProtoMember(11)] public long Const2 { get; set; }
    
    [ProtoMember(12)] public long Const3 { get; set; }
    
    [ProtoMember(13)] public long LastSyncTime { get; set; }
    
    [ProtoMember(14)] public long Const4 { get; set; }
}