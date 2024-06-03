using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class FriendRequest
{
    [ProtoMember(1)] public FriendRequestInfo? Info { get; set; }
}

[ProtoContract]
internal class FriendRequestInfo
{
    [ProtoMember(1)] public string TargetUid { get; set; }
    
    [ProtoMember(2)] public string SourceUid { get; set; }
    
    [ProtoMember(10)] public string Message { get; set; }
    
    [ProtoMember(11)] public string Source { get; set; }
}