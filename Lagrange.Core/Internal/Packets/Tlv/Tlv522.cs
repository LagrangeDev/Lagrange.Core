using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x522)]
internal class Tlv522 : TlvBody
{
    [BinaryProperty] public uint u1 { get; set; }

    [BinaryProperty] public uint Uin { get; set; }

    [BinaryProperty] public uint Ip { get; set; }

    [BinaryProperty] public uint Time { get; set; }

    [BinaryProperty] public uint SubAppId { get; set; }
}