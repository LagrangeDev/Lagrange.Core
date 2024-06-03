using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

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

    protected override BinaryPacket ConstructTlv() => new BinaryPacket()
        .WriteUshort(0)
        .WriteUint((uint)AppInfo.AppId)
        .WriteUlong(0) // uin
        .WriteBytes(Array.Empty<byte>()) // TGT
        .WriteByte(0)
        .WriteBytes(Array.Empty<byte>(), Prefix.Uint16 | Prefix.LengthOnly)
        .WritePacket(TlvPacker.PackQrCode(Keystore.Session.UnusualSign == null ? ConstructTlvs : ConstructTlvsPassword));

    public static Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, BotKeystore keystore, out byte[] signature)
    {
        byte dummy = packet.ReadByte();
        signature = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly).ToArray();
        keystore.Session.QrSign = signature;
        
        return TlvPacker.ReadTlvCollections(packet, true);
    }
}