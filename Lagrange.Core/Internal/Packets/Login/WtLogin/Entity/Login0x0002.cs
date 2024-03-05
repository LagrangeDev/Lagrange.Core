using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class Login0x0002 : Login
{
    private const ushort LoginCommand = 0x0002;

    public Login0x0002(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(LoginCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructLogin()
    {
        var (ticket, _, _) = Keystore.Session.Captcha!.Value;

        return new BinaryPacket()
            .WriteShort(0x0008, false)
            .WritePacket(new TlvPacket(0x193, new Tlv193(ticket)))
            .WritePacket(new TlvPacket(0x008, new Tlv008()))
            .WritePacket(new TlvPacket(0x104, new Tlv104(Keystore)))
            .WritePacket(new TlvPacket(0x116, new Tlv116(AppInfo)))
            .WritePacket(new TlvPacket(0x547, new Tlv547(Keystore)))
            .WritePacket(new TlvPacket(0x544, new Tlv544(AppInfo, Device, Keystore.Uin, Cmd, LoginCommand)))
            .WritePacket(new TlvPacket(0x553, new Tlv553(AppInfo, Keystore)))
            .WritePacket(new TlvPacket(0x542, new Tlv542()));
    }
}

