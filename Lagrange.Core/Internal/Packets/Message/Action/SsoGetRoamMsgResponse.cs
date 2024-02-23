using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class SsoGetRoamMsgResponse
{
    [ProtoMember(3)] public string FriendUid { get; set; }
    
    [ProtoMember(5)] public uint Timestamp { get; set; }

    [ProtoMember(6)] public uint Random { get; set; }
    
    [ProtoMember(7)] public List<PushMsgBody> Messages { get; set; }
}