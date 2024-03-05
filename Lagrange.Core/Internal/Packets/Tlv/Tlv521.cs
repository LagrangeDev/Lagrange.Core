using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x521)]
internal class Tlv521 : TlvBody
{
    public Tlv521(BotAppInfo appInfo)
    {
        if (appInfo.Os == "Android")
        {
            ProductType = 0;
            ProductDesc = "";
        }
        else
        {
            ProductType = 0x13;
            ProductDesc = "basicim";
        }
    }
    [BinaryProperty] public uint ProductType { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string ProductDesc { get; set; }
}