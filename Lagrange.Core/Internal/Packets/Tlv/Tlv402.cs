using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x402, true)]
internal class Tlv402Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] T402 { get; set; }
}