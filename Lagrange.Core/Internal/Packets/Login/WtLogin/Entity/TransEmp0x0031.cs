using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class TransEmp0x0031 : TransEmp
{
    private const ushort QrCodeCommand = 0x0031;

    public TransEmp0x0031(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(QrCodeCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructTransEmp()
    {
        var packet = new BinaryPacket()
            .WriteUshort(0, false)
            .WriteUlong(0, false)
            .WriteByte(0);
        if (Keystore.Session.UnusualSign != null)
            packet.WritePacket(new TlvPacket(0x011, new Tlv011(Keystore)));

        packet.WritePacket(new TlvPacket(0x016, new Tlv016(AppInfo, Device)))
            .WritePacket(new TlvPacket(0x01B, new Tlv01B()))
            .WritePacket(new TlvPacket(0x01D, new Tlv01D(AppInfo)))
            .WritePacket(new TlvPacket(0x033, new Tlv033(Device)))
            .WritePacket(new TlvPacket(0x035, new Tlv035(AppInfo)))
            .WritePacket(new TlvPacket(0x066, new Tlv066(AppInfo)))
            .WritePacket(new TlvPacket(0x0D1, new Tlv0D1(AppInfo, Device)));

        return packet;
    }

    public static Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, BotKeystore keystore, out byte[] signature)
    {
        byte dummy = packet.ReadByte();
        signature = packet.ReadBytes(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
        keystore.Session.QrSign = signature;

        return TlvPacker.ReadTlvCollections(packet);
    }
}