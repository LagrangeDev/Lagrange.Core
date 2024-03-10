using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x004)]
internal class Tlv004 : TlvBody
{
    [BinaryProperty(Prefix.Uint32 | Prefix.LengthOnly)] public string Uin { get; set; }
}