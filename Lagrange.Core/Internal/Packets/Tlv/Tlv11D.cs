using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x11D)]
internal class Tlv11D : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] u1 { get; set; }
}