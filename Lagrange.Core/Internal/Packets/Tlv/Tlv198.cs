using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x198)]
internal class Tlv198 : TlvBody
{
    [BinaryProperty] public byte u1 { get; set; } = 0;
}