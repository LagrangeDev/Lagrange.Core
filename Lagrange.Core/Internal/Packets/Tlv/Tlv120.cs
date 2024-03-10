using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x120)]
internal class Tlv120 : TlvBody
{
    /// <summary>
    /// The encrypted TLV data that should be decrypted with the TGTGT key.
    /// </summary>
    [BinaryProperty(Prefix.None)] public string Skey { get; set; }
}