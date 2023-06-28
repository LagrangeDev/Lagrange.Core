using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[TlvQrCode(0x01e)]
internal class TlvQrCode1E : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] TgtgtKey { get; set; } // a1_sig
}