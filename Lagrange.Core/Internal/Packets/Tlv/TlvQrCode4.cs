using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[TlvQrCode(0x004)]
internal class TlvQrCode4 : TlvBody
{
    [BinaryProperty] public ushort Type { get; set; }
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Uin { get; set; }
}