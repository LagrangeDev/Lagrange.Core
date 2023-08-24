using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Routing;

[ProtoContract]
internal class ResponseForward
{
    [ProtoMember(6)] public string? FriendName { get; set; }
}