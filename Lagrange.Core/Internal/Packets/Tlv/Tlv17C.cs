using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x17C)]
internal class Tlv17C : TlvBody
{
    public Tlv17C(BotKeystore keystore) => SmsCode = keystore.Session.SmsCode ?? "";
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string SmsCode { get; set; }
}