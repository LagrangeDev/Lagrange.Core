using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

/// <summary>
/// trpc.msg.register_proxy.RegisterProxy.SsoGetC2cMsg
/// </summary>
[ProtoContract]
internal class SsoGetC2cMsg
{
    [ProtoMember(2)] public string? FriendUid { get; set; }

    [ProtoMember(3)] public ulong StartSequence { get; set; }

    [ProtoMember(4)] public ulong EndSequence { get; set; }
}