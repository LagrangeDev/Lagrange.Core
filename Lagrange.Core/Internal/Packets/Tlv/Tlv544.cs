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
        /*
        var salt = new BinaryPacket();
            if (v == 2) // Tlv544v2
            {
                salt.WriteUint(0, false)
                    .WriteBytes(device.System.Guid.ToByteArray(), Prefix.Uint16 | Prefix.LengthOnly)
                    .WriteBytes(Encoding.ASCII.GetBytes(appInfo.WtLoginSdk.SdkVersion), Prefix.Uint16 | Prefix.LengthOnly)
                    .WriteUint(subCmd, false)
                    .WriteUint(0, false);
            }
            else
            {
                salt.WriteUint(Uin, false)
                    .WriteBytes(device.System.Guid.ToByteArray(), Prefix.Uint16 | Prefix.LengthOnly)
                    .WriteBytes(Encoding.ASCII.GetBytes(appInfo.WtLoginSdk.SdkVersion), Prefix.Uint16 | Prefix.LengthOnly)
                    .WriteUint(subCmd, false);
            }
            SignT544 = Algorithm.Sign((uint)(DateTime.UtcNow.Ticks / 10), salt.ToArray());
        */
        // Todo: get AndroidSigner
        SignT544 = appInfo.SignProvider.Energy(appInfo.WtLoginSdk.SdkVersion, Uin, device.System.Guid, $"{cmd:x}_{subCmd:x}");
    }
    [BinaryProperty(Prefix.None)] public byte[] SignT544 { get; set; }
}