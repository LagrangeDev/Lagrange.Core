using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x118)]
internal class Tlv118 : TlvBody
{
    [BinaryProperty] public uint u1 { get; set; }
    [BinaryProperty(Prefix.Uint8)] public string u2 { get; set; }
}