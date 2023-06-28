using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x166)]
internal class Tlv166 : TlvBody
{
    [BinaryProperty] public byte ImageType { get; set; } = 5;
}