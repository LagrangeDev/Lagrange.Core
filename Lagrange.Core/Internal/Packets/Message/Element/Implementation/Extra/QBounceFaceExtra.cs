#pragma warning disable CS8618

using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class QBounceFaceExtra
{
    [ProtoMember(1)] public int Field1 { get; set; } = 13;

    [ProtoMember(2)] public uint Count { get; set; } = 1;

    [ProtoMember(3)] public string Name { get; set; } = string.Empty;

    [ProtoMember(6)] public QSmallFaceExtra Face { get; set; }

    [ProtoContract]
    public class FallbackPreviewTextPb
    {
        [ProtoMember(1)] public string Text { get; set; } = string.Empty;
    }
}