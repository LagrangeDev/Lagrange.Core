using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation.Extra;

/// <summary>
/// <see cref="Face"/>
/// </summary>
[ProtoContract]
internal class FaceExtra
{
    [ProtoMember(1)] public int? FaceId { get; set; }
}