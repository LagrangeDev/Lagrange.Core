using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x011)]
internal class Tlv011 : TlvBody
{
    public Tlv011(BotKeystore keystore)
    {
        UnusualSign = keystore.Session.UnusualSign!;
    }

    [BinaryProperty(Prefix.None)] public byte[] UnusualSign { get; set; }
}