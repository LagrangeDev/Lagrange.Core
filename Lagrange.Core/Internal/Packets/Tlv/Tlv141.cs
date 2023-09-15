using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x141)]
internal class Tlv141 : TlvBody
{
    [BinaryProperty(Prefix.Uint32 | Prefix.LengthOnly)] public string Unknown { get; set; } = "Unknown";

    [BinaryProperty] public uint Const0 { get; set; } = 0;
}