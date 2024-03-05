using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x133)]
internal class Tlv133 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] WtSessionTicket { get; set; }
}