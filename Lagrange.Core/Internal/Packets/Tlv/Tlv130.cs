using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x130)]
internal class Tlv130 : TlvBody
{
    [BinaryProperty] public ushort u1 { get; set; }

    [BinaryProperty] public uint Time { get; set; }

    [BinaryProperty] public uint Ip { get; set; }

    [BinaryProperty] public uint u2 { get; set; }

}