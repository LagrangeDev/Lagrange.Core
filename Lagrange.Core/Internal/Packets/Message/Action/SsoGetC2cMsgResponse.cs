using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class SsoGetC2cMsgResponse
{
    [ProtoMember(1)] public uint Retcode { get; set; }

    [ProtoMember(2)] public string Message { get; set; }

    [ProtoMember(4)] public string FriendUid { get; set; }

    [ProtoMember(7)] public List<PushMsgBody>? Messages { get; set; }
}