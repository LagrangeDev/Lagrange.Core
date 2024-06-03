using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x144)]
[TlvEncrypt(TlvEncryptAttribute.KeyType.TgtgtKey)]
internal class Tlv144 : TlvBody
{
    public Tlv144(BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        TlvCount = 4;
        Tlv16E = new TlvPacket(0x16E, new Tlv16E(deviceInfo)).ToArray();
        Tlv147 = new TlvPacket(0x147, new Tlv147(appInfo)).ToArray();
        Tlv128 = new TlvPacket(0x128, new Tlv128(appInfo, deviceInfo)).ToArray();
        Tlv124 = new TlvPacket(0x124, new Tlv124()).ToArray();
    }
    
    [BinaryProperty] public ushort TlvCount { get; set; }
    
    [BinaryProperty] public byte[] Tlv16E { get; set; }
    
    [BinaryProperty] public byte[] Tlv147 { get; set; }
    
    [BinaryProperty] public byte[] Tlv128 { get; set; }
    
    [BinaryProperty] public byte[] Tlv124 { get; set; }
}