using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x16D)]
internal class Tlv16D : TlvBody
{
    [BinaryProperty(Prefix.None)] public string SuperKey { get; set; }
}