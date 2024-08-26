using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0X11A)]
internal class Tlv11A : TlvBody
{
    [BinaryProperty] public ushort FaceId { get; set; }

    [BinaryProperty] public byte Age { get; set; }

    [BinaryProperty] public byte Gender { get; set; }

    [BinaryProperty(Prefix.Uint8 | Prefix.None)] public string Nickname { get; set; }
}