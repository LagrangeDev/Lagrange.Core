using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

/// <summary>
/// trpc.msg.register_proxy.RegisterProxy.SsoGetRoamMsg
/// </summary>
[ProtoContract]
internal class SsoGetRoamMsg
{
    [ProtoMember(1)] public string? FriendUid { get; set; }
    
    [ProtoMember(2)] public uint Time { get; set; }
    
    [ProtoMember(3)] public uint Random { get; set; }  // 0
    
    [ProtoMember(4)] public uint Count { get; set; }
    
    [ProtoMember(5)] public bool Direction { get; set; }  // true
}