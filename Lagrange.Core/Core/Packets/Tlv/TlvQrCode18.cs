using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[TlvQrCode(0x018)]
internal class TlvQrCode18 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] TempPassword { get; set; }
}