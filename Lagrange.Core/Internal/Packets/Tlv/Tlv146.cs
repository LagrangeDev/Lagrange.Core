using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x146)]
internal class Tlv146 : TlvBody
{
    [BinaryProperty] public uint State { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Tag { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Message { get; set; }

    [BinaryProperty] public uint Field0 { get; set; }
}