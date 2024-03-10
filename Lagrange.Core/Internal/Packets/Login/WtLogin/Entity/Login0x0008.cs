using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class Login0x0008 : Login
{
    private const ushort LoginCommand = 0x0008;

    public Login0x0008(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(LoginCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructLogin() => new BinaryPacket()
        .WriteShort(0x0008, false)
        .WritePacket(new TlvPacket(0x008, new Tlv008()))
        .WritePacket(new TlvPacket(0x104, new Tlv104(Keystore)))
        .WritePacket(new TlvPacket(0x116, new Tlv116(AppInfo)))
        .WritePacket(new TlvPacket(0x174, new Tlv174(Keystore)))
        .WritePacket(new TlvPacket(0x17A, new Tlv17A()))
        .WritePacket(new TlvPacket(0x197, new Tlv197()))
        .WritePacket(new TlvPacket(0x542, new Tlv542()))
        .WritePacket(new TlvPacket(0x553, new Tlv553(AppInfo, Keystore)));
}

