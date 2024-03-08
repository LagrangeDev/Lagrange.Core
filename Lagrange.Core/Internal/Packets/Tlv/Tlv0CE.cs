using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x0CE)]
internal class Tlv0CE : TlvBody
{
    [BinaryProperty] public uint u1 { get; set; }
}