using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x10E)]
internal class Tlv10E : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] StKey { get; set; }
}