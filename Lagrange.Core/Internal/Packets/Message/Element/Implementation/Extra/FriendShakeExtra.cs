using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class FriendShakeExtra
{
    [ProtoMember(1)] public ushort FaceId { get; set; }

    [ProtoMember(7)] public ushort Strength { get; set; }
}
