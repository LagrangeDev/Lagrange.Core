using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x537)]
internal class Tlv537 : TlvBody
{
    [BinaryProperty] public ushort u1 { get; set; }

    [BinaryProperty] public uint u2 { get; set; }

    [BinaryProperty] public uint Uin { get; set; }

    [BinaryProperty] public byte u3 { get; set; }

    [BinaryProperty] public uint Ip { get; set; }

    [BinaryProperty] public uint Time { get; set; }

    [BinaryProperty] public uint SubAppId { get; set; }
}