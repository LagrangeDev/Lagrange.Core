using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x147)]
internal class Tlv147 : TlvBody
{
    public Tlv147(BotAppInfo appInfo)
    {
        AppId = (uint)appInfo.AppId;
        PtVersion = appInfo.PtVersion;
        PackageName = appInfo.PackageName;
    }

    [BinaryProperty] public uint AppId { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PtVersion { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PackageName { get; set; }
}