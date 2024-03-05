using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x197)]
internal class Tlv197 : TlvBody
{
    [BinaryProperty] public byte u1 { get; set; } = 0;
}