using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0X143)]
internal class Tlv143 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] D2 { get; set; }
}