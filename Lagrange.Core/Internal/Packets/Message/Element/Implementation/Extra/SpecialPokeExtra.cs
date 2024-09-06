using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class SpecialPokeExtra
{
    [ProtoMember(1)] public uint Type { get; set; }

    [ProtoMember(2)] public uint Count { get; set; }

    [ProtoMember(3)] public string FaceName { get; set; } = string.Empty;

    // [ProtoMember(6)] public byte[] PbReserve { get; set; } = Array.Empty<byte>();
}
