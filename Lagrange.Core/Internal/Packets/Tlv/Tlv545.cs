using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utils.Tencent;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x545)]
internal class Tlv545 : TlvBody
{
    public Tlv545(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        if (keystore.Session.QImei == null)
            keystore.Session.QImei = QImeiProvider.RequestQImei(deviceInfo, appInfo).Result;
        var qImei36 = keystore.Session.QImei?.Q36;
        QImei = qImei36 != null ? qImei36 : deviceInfo.Model.Imei;
    }
    [BinaryProperty(Prefix.None)] public string QImei { get; set; }
}