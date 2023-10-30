using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class FriendRequest
{
    [ProtoMember(1)] public FriendRequestInfo Info { get; set; }
}

[ProtoContract]
internal class FriendRequestInfo
{
    [ProtoMember(1)] public uint Field0 { get; set; }
    
    [ProtoMember(2)] public string SourceUid { get; set; }
    
    [ProtoMember(3)] public string Message { get; set; }
    
    [ProtoMember(4)] public string Name { get; set; }
    
    [ProtoMember(5)] public uint Field5 { get; set; }
}