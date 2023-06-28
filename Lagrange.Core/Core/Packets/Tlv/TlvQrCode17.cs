using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[TlvQrCode(0x017)]
internal class TlvQrCode17 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] QrCode { get; set; }
}