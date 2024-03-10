using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x512)]
internal class Tlv512 : TlvBody
{
    [BinaryProperty] public ushort DomainCount { get; set; }

    [BinaryProperty(Prefix.None)] public byte[] DomainBody { get; set; }
}