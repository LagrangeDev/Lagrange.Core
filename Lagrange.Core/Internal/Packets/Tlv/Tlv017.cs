using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x017)]
internal class Tlv017 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] QrCode { get; set; }
}