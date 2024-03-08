using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class Login0x0009V2 : Login
{
    private const ushort LoginCommand = 0x0009;

    public Login0x0009V2(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(LoginCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructLogin() => new BinaryPacket()
        .WriteShort(0x000F, false)
        .WritePacket(new TlvPacket(0x106, new Tlv106V2(Keystore)))
        .WritePacket(new TlvPacket(0x144, new Tlv144(Device), (Keystore.TeaImpl, Keystore.Stub.TgtgtKey)))
        .WritePacket(new TlvPacket(0x116, new Tlv116(AppInfo)))
        .WritePacket(new TlvPacket(0x142, new Tlv142(AppInfo)))
        .WritePacket(new TlvPacket(0x145, new Tlv145(Device)))
        .WritePacket(new TlvPacket(0x018, new Tlv018(Keystore, AppInfo)))
        .WritePacket(new TlvPacket(0x141, new Tlv141(Device)))
        .WritePacket(new TlvPacket(0x177, new Tlv177(AppInfo)))
        .WritePacket(new TlvPacket(0x191, new Tlv191(0x82)))
        .WritePacket(new TlvPacket(0x100, new Tlv100(AppInfo)))
        .WritePacket(new TlvPacket(0x107, new Tlv107()))
        .WritePacket(new TlvPacket(0x318, new Tlv318()))
        .WritePacket(new TlvPacket(0x16A, new Tlv16A(Keystore)))
        .WritePacket(new TlvPacket(0x166, new Tlv166()))
        .WritePacket(new TlvPacket(0x521, new Tlv521(AppInfo)));
}