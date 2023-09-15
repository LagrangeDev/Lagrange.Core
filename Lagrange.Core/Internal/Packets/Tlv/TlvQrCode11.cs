using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[TlvQrCode(0x11)]
internal class TlvQrCode11 : TlvBody
{
    public TlvQrCode11(BotKeystore keystore)
    {
        UnusualSign = keystore.Session.UnusualSign ?? throw new InvalidOperationException();
    }
    
    [BinaryProperty(Prefix.None)] public byte[] UnusualSign { get; set; }
}