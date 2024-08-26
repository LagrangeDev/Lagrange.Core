using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x141)]
internal class Tlv141 : TlvBody
{
    [BinaryProperty] public ushort Version { get; set; } = 0;

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Unknown { get; set; } = "Unknown";

    [BinaryProperty] public ushort NetworkType { get; set; } = 0;

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Apn { get; set; } = "";
}