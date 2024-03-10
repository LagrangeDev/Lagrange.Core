using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x108)]
internal class Tlv108 : TlvBody
{
    public Tlv108(BotKeystore keystore)
    {
        Ksid = keystore.Session.Ksid;
    }
    [BinaryProperty(Prefix.None)] public byte[] Ksid { get; set; }
}

[Tlv(0x108, true)]
internal class Tlv108Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] Ksid { get; set; }
}