using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x124)]
internal class Tlv124 : TlvBody
{
    public Tlv124(BotDeviceInfo deviceInfo)
    {
        OsType = deviceInfo.System.OsType;
        OsVersion = deviceInfo.System.OsVersion;
        NetworkType = (ushort)deviceInfo.Network.NetworkType;
        NetworkName = deviceInfo.Network.NetworkName;
        Apn = deviceInfo.Network.Apn;
    }
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string OsType { get; set; }
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string OsVersion { get; set; }
    [BinaryProperty] public ushort NetworkType { get; set; }
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string NetworkName { get; set; }
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Const1 { get; set; } = "";
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Apn { get; set; }

}