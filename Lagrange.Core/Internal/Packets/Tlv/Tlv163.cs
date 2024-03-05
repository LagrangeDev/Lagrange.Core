using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x163)]
internal class Tlv163 : TlvBody
{
    [BinaryProperty(Prefix.None)] public string u1 { get; set; } //size: 16
}