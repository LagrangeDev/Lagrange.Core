using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x178, true)]
internal class Tlv178 : TlvBody
{
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string AreaCode { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PhoneNumber { get; set; }

    [BinaryProperty] public uint u1 { get; set; }

    [BinaryProperty] public ushort u2 { get; set; }

    [BinaryProperty] public ushort u3 { get; set; }
}