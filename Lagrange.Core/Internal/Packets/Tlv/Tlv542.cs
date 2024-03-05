using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x542)]
internal class Tlv542 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] Unknown1 { get; set; } = new byte[] { 0x4A, 0x07, 0x60, 0x01, 0x78, 0x01, 0x80, 0x01, 0x01 }; // empty when send sms, submit sms
}