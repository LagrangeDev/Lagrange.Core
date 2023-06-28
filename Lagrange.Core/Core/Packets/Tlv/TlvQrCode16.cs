using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Core.Packets.Tlv;

[TlvQrCode(0x016)]
internal class Tlv16QrCode : TlvBody
{
    public Tlv16QrCode(BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        SubAppId = (uint)appInfo.AppId;
        AppIdQrCode = (uint)appInfo.AppIdQrCode;
        Guid = deviceInfo.Guid.ToByteArray();
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