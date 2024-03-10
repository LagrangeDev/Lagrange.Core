using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x007)]
internal class Tlv007 : TlvBody
{
    [BinaryProperty] public uint u1 { get; set; }
}