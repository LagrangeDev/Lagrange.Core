using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x403, true)]
internal class Tlv403Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] T403 { get; set; }
}