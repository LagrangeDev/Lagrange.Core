using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x17D, true)]
internal class Tlv17D : TlvBody
{
    [BinaryProperty] public ushort u1 { get; set; }

    [BinaryProperty] public uint u2 { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Url { get; set; }
}