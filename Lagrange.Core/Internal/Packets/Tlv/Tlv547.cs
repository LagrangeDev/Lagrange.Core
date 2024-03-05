using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Tencent;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x547)]
internal class Tlv547 : TlvBody
{
    public Tlv547(BotKeystore keystore)
    {

        if (keystore.Session.PowValue == null)
            PowValue = Array.Empty<byte>();
        else 
            PowValue = ClientPow.GetPow(keystore.Session.PowValue);
    }
    [BinaryProperty(Prefix.None)] public byte[] PowValue { get; set; }
}