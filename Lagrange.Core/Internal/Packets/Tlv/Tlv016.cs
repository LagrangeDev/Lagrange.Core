using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x016)]
internal class Tlv016 : TlvBody
{
    public Tlv016(BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        SubAppId = appInfo.AppId;
        AppIdQrCode = appInfo.AppIdQrCode;
        Guid = deviceInfo.System.Guid.ToByteArray();
        PackageName = appInfo.PackageName;
        PtVersion = appInfo.PtVersion;
        PackageName2 = appInfo.PackageName;
    }

    [BinaryProperty] public uint Field0 { get; } = 0; // unknown
    
    [BinaryProperty] public uint SubAppId { get; }
    
    [BinaryProperty] public uint AppIdQrCode { get; }
    
    [BinaryProperty(Prefix.None)] public byte[] Guid { get; }
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PackageName { get; }
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PtVersion { get; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PackageName2 { get; }
}