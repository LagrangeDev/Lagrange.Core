using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x104)]
internal class Tlv104 : TlvBody
{
    public Tlv104(BotKeystore keystore) => T104 = keystore.Session.T104 ?? "";
    [BinaryProperty(Prefix.None)] public string T104 { get; set; }
}

[Tlv(0x104, true)]
internal class Tlv104Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public string T104 { get; set; }
}