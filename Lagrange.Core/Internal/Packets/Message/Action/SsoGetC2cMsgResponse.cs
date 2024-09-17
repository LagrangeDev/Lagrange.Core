using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class SsoGetC2cMsgResponse
{
    [ProtoMember(4)] public string FriendUid { get; set; }

    [ProtoMember(7)] public List<PushMsgBody>? Messages { get; set; }
}