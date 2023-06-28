using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x177)]
internal class Tlv177 : TlvBody
{
    public Tlv177(BotAppInfo appInfo) => WtLoginSdk = appInfo.WtLoginSdk;

    [BinaryProperty] public byte Field1 { get; set; } = 1;

    [BinaryProperty] public uint BuildTime { get; set; } = 0; // const 0, not sure what this is

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string WtLoginSdk { get; set; }
}