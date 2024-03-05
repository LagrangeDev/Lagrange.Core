using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x128)]
internal class Tlv128 : TlvBody
{
    public Tlv128(BotDeviceInfo deviceInfo)
    {
        DeviceName = deviceInfo.Model.DeviceName;
        Guid = deviceInfo.System.Guid.ToByteArray();
        Brand = deviceInfo.Model.Brand;
    }
    
    [BinaryProperty] public ushort Const0 { get; set; } = 0;
    
    [BinaryProperty] public byte GuidNew { get; set; } = 0;
    
    [BinaryProperty] public byte GuidAvailable { get; set; } = 1;
    
    [BinaryProperty] public byte GuidChanged { get; set; } = 0;
    
    [BinaryProperty] public uint GuidFlag { get; set; } = 0x01000000;
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string DeviceName { get; set; }
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public byte[] Guid { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Brand { get; set; }
}