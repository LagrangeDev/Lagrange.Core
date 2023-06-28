using Lagrange.Core.Common;
using Lagrange.Core.Core.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Core.Packets.Login.WtLogin.Entity;

internal class TransEmp31 : TransEmp
{
    private const ushort QrCodeCommand = 0x31;
    
    private static readonly ushort[] ConstructTlvs =
    { 
        0x016, 0x01B, 0x01D, 0x033, 0x035, 0x066, 0x0D1
    };

    private static readonly ushort[] ConstructTlvsPassword =
    { 
        0x011, 0x016, 0x01B, 0x01D, 0x033, 0x035, 0x066, 0x0D1
    };

    public TransEmp31(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(QrCodeCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructTransEmp() => new BinaryPacket()
        .WriteUshort(0, false)
        .WriteUlong(0, false)
        .WriteByte(0)
        .WritePacket(TlvPacker.PackQrCode(Keystore.Session.UnusualSign == null ? ConstructTlvs : ConstructTlvsPassword))
        .WriteByte(0x03);

    public static Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, BotKeystore keystore, out byte[] signature)
    {
        byte dummy = packet.ReadByte();
        signature = packet.ReadBytes(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
        keystore.Session.QrSign = signature;
        
        return TlvPacker.ReadTlvCollections(packet, true);
    }
}