using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x174)]
internal class Tlv174 : TlvBody
{
    public Tlv174(BotKeystore keystore) => T174 = keystore.Session.T174 ?? Array.Empty<byte>();

    [BinaryProperty(Prefix.None)] public byte[] T174 { get; set; }
}

[Tlv(0x174, true)]
internal class Tlv174Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] T174 { get; set; }
}