using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal abstract class Login : WtLoginBase
{
    private readonly ushort _loginCommand;

    private const string PacketCommand = "wtlogin.login";
    private const ushort WtLoginCommand = 2064;
    private const byte WtLoginCmdVer = 135;
    private const byte WtLoginPubId = 2;

    public Login(ushort loginCommand, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(PacketCommand, WtLoginCommand, WtLoginCmdVer, WtLoginPubId, keystore, appInfo, device)
        => _loginCommand = loginCommand;

    protected override BinaryPacket ConstructBody()
    {
        var packet = new BinaryPacket()
            .WriteUshort(_loginCommand, false)
            .WritePacket(ConstructLogin());

        return packet;
    }

    public Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, BotKeystore keystore, out ushort loginCommand, out State state)
    {
        packet = DeserializePacket(keystore, packet);

        loginCommand = packet.ReadUshort(false);
        state = (State)packet.ReadByte();
        return TlvPacker.ReadTlvCollections(packet);
    }

    protected abstract BinaryPacket ConstructLogin();


    public enum State : byte
    {
        Success = 0,
        CaptchaVerify = 2,
        SmsRequired = 160,
        DeviceLock = 204,
        DeviceLockViaSmsNewArea = 239,

        PreventByIncorrectPassword = 1,
        PreventByReceiveIssue = 3,
        PreventByTokenExpired = 15,
        PreventByAccountBanned = 40,
        PreventByOperationTimeout = 155,
        PreventBySmsSentFailed = 162,
        PreventByIncorrectSmsCode = 163,
        PreventByLoginDenied = 167,
        PreventByOutdatedVersion = 235,
        PreventByHighRiskOfEnvironment = 237,
        Unknown = 240,
    }
}