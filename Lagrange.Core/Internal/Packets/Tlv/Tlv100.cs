using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x100)]
internal class Tlv100 : TlvBody
{
    public Tlv100(BotAppInfo appInfo)
    {
        AppId = appInfo.AppId;
        SubAppId = appInfo.SubAppId;
        AppClientVersion = appInfo.AppClientVersion;
        MainSigMap = appInfo.WtLoginSdk.MainSigBitmap;
    }
    
    [BinaryProperty] public ushort DbBufVersion { get; set; } = 1;
    
    [BinaryProperty] public uint SsoVersion { get; set; } = 0x00000015;
    
    [BinaryProperty] public uint AppId { get; set; }
    
    [BinaryProperty] public uint SubAppId { get; set; }
    
    [BinaryProperty] public uint AppClientVersion { get; set; }
    
    [BinaryProperty] public uint MainSigMap { get; set; }
}