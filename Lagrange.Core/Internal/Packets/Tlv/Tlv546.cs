using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x546, true)]
internal class Tlv546 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] PowValue { get; set; }
}