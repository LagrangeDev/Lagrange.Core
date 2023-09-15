using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[TlvQrCode(0x019)]
internal class TlvQrCode19 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] NoPicSig { get; set; }
}