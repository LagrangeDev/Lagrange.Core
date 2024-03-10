using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x01e)]
internal class Tlv01E : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] TgtgtKey { get; set; } // a1_sig
}