using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x134)]
internal class Tlv134 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] WtSessionTicketKey { get; set; }
}