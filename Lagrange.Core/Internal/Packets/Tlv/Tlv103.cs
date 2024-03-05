using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x103)]
internal class Tlv103 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] StWebSig { get; set; }
}