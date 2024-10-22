using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

/// <summary>
/// Constructed at <see cref="CommonElem"/>, Service Type 33, Small face (FaceId >= 260)
/// </summary>
[ProtoContract]
internal class QSmallFaceExtra
{
    [ProtoMember(1)] public uint FaceId { get; set; }

    [ProtoMember(2)] public string? Text { get; set; }

    [ProtoMember(3)] public string? CompatText { get; set; }
}