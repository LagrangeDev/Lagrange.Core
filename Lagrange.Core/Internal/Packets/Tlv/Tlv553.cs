using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x553)]
internal class Tlv553 : TlvBody
{
    public Tlv553(BotAppInfo appInfo,BotKeystore keystore)
    {
        // qSec.getFeKitAttach(u.w, "", "0x810", "0x12")
        XwDebugId = appInfo.SignProvider.GetXwDebugId(keystore.Uin, "810_12");
    }

    [BinaryProperty(Prefix.None)] public byte[]? XwDebugId { get; set; }
}