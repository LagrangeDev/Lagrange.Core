using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x177)]
internal class Tlv177 : TlvBody
{
    public Tlv177(BotAppInfo appInfo)
    {
        SdkBuildTime = appInfo.WtLoginSdk.SdkBuildTime;
        SdkVersion = appInfo.WtLoginSdk.SdkVersion;
    }

    [BinaryProperty] public byte Const1 { get; set; } = 1;

    [BinaryProperty] public uint SdkBuildTime { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string SdkVersion { get; set; }
}