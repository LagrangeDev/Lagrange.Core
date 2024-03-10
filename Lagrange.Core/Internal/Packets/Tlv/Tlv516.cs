using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x516)]
internal class Tlv516 : TlvBody
{
    [BinaryProperty] public uint u1 { get; set; } = 0;
}