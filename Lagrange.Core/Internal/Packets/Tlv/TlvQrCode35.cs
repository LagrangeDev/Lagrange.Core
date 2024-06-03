using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[TlvQrCode(0x035)]
internal class TlvQrCode35 : TlvBody
{
    public TlvQrCode35(BotAppInfo appInfo) => PtOsVersion = appInfo.PtOsVersion;

    [BinaryProperty] public int PtOsVersion { get; }
}