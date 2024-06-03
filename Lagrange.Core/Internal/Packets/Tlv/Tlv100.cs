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
        AppId = (uint)appInfo.AppId;
        SubAppId = (uint)appInfo.SubAppId;
        AppClientVersion = appInfo.AppClientVersion;
        MainSigMap = appInfo.MainSigMap;
    }
    
    [BinaryProperty] public ushort DbBufVersion { get; set; } = 0; // originally 0x1
    
    [BinaryProperty] public uint SsoVersion { get; set; } = 0x00000005;
    
    [BinaryProperty] public uint AppId { get; set; }
    
    [BinaryProperty] public uint SubAppId { get; set; }
    
    [BinaryProperty] public uint AppClientVersion { get; set; }
    
    [BinaryProperty] public uint MainSigMap { get; set; }
}