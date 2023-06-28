using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x191)]
internal class Tlv191 : TlvBody
{
    [BinaryProperty] public byte K { get; set; } = 0; // originally 0x82
}