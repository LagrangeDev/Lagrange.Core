using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class FriendShakeExtra
{
    [ProtoMember(1)] public uint FaceId { get; set; }

    [ProtoMember(7)] public uint Strength { get; set; }
}
