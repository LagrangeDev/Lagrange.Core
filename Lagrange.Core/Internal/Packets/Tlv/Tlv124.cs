using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x124)]
internal class Tlv124 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] Field0 { get; set; } = new byte[12];
}