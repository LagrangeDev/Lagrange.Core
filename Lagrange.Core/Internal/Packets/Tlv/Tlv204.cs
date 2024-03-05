using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x204, true)]
internal class Tlv204 : TlvBody
{
    [BinaryProperty(Prefix.None)] public string Url { get; set; }
}