using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x141)]
internal class Tlv141 : TlvBody
{
    public Tlv141(BotDeviceInfo deviceInfo)
    {
        NetworkName = deviceInfo.Network.NetworkName;
        NetworkType = (ushort)deviceInfo.Network.NetworkType;
        Apn = deviceInfo.Network.Apn;
    }
    [BinaryProperty] public ushort Version { get; set; } = 1;
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string NetworkName { get; set; }

    [BinaryProperty] public ushort NetworkType { get; set; }
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Apn { get; set; }
}