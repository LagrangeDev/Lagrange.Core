using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x521)]
internal class Tlv521 : TlvBody
{
    [BinaryProperty] public uint ProductType { get; set; } = 0x13;

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string ProductDesc { get; set; } = "basicim";
}