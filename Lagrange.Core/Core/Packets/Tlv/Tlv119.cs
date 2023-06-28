using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x119)]
internal class Tlv119 : TlvBody
{
    /// <summary>
    /// The encrypted TLV data that should be decrypted with the TGTGT key.
    /// </summary>
    [BinaryProperty(Prefix.None)] public byte[] EncryptedTlv { get; set; }
}