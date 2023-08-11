using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service;

/// <summary>
/// Body for trpc.msg.register_proxy.RegisterProxy.InfoSyncPush type 5, as List&lt;InfoPushGroup&gt;
/// </summary>
[ProtoContract]
internal class InfoPushGroup
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Sequence1 { get; set; }
    
    [ProtoMember(3)] public uint Sequence2 { get; set; }
    
    [ProtoMember(4)] public uint Field4 { get; set; }
    
    [ProtoMember(8)] public uint LastMsgTime { get; set; }

    [ProtoMember(9)] public string GroupName { get; set; } = "";
    
    [ProtoMember(10)] public uint Sequence3 { get; set; }
    
    [ProtoMember(11)] public ulong Random { get; set; }
    
    [ProtoMember(13)] public uint Field13 { get; set; }
}