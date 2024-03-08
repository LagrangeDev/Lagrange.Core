using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x544)]
internal class Tlv544 : TlvBody
{
    public Tlv544(BotAppInfo appInfo, BotDeviceInfo device, uint Uin, uint cmd, uint subCmd)
    {
        SignT544 = appInfo.SignProvider.Energy(appInfo.WtLoginSdk.SdkVersion, Uin, device.System.Guid, $"{cmd:x}_{subCmd:x}");
    }
    [BinaryProperty(Prefix.None)] public byte[] SignT544 { get; set; }
}