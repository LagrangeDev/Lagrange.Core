using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal abstract class ExchangeEmp : WtLoginBase
{
    private readonly ushort _exchangeCommand;

    private const string PacketCommand = "wtlogin.exchange_emp";
    private const ushort WtLoginCommand = 2064;
    private const byte WtLoginCmdVer = 69;
    private const byte WtLoginPubId = 2;

    protected ExchangeEmp(ushort exchangeCommand, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(PacketCommand, WtLoginCommand, WtLoginCmdVer, WtLoginPubId, keystore, appInfo, device) 
        => _exchangeCommand = exchangeCommand;

    protected override BinaryPacket ConstructBody()
    {
        var packet = new BinaryPacket()
            .WriteUshort(_exchangeCommand, false)
            .WritePacket(ConstructExchangeEmp());

        return packet;
    }

    public Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, BotKeystore keystore, out ushort exchangeCommand, out State state)
    {
        packet = DeserializePacket(keystore, packet);

        exchangeCommand = packet.ReadUshort(false);
        state = (State)packet.ReadByte();
        return TlvPacker.ReadTlvCollections(packet);
    }

    protected abstract BinaryPacket ConstructExchangeEmp();

    public enum State : byte
    {
        Success = 0
    }
}
