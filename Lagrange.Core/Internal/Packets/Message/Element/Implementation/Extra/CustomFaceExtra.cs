using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class CustomFaceExtra
{
    [ProtoMember(31)] public string? Hash { get; set; }
}