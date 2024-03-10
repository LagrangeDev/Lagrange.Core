using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x114)]
internal class Tlv114 : TlvBody
{
    [BinaryProperty] public ushort u1 { get; set; }

    [BinaryProperty] public uint u2 { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public ushort u3 { get; set; }
}