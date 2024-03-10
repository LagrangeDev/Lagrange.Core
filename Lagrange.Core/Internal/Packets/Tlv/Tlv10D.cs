using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x10D)]
internal class Tlv10D : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] TgtKey { get; set; }
}