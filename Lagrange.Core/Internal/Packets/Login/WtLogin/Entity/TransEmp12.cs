using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class TransEmp12 : TransEmp
{
    private const ushort QrCodeCommand = 0x12;
    
    public TransEmp12(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(QrCodeCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructTlv()
    {
        if (Keystore.Session.QrSign != null)
        {
            return new BinaryPacket()
                .WriteUshort(0)
                .WriteUint((uint)AppInfo.AppId)
                .WriteBytes(Keystore.Session.QrSign, Prefix.Uint16 | Prefix.LengthOnly)
                .WriteUlong(0) // uin
                .WriteByte(0) // version
                .WriteBytes(Array.Empty<byte>(), Prefix.Uint16 | Prefix.LengthOnly)
                .WriteShort(0);
        }

        throw new Exception("QrSign is null");
    }

    public static Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, out State qrState)
    {
        if ((qrState = (State)packet.ReadByte()) == State.Confirmed)
        {
            packet.Skip(12); // misc unknown data
            return TlvPacker.ReadTlvCollections(packet, true);
        }

        return new Dictionary<ushort, TlvBody>();
    }

    internal enum State : byte
    {
        Confirmed = 0,
        CodeExpired = 17,
        WaitingForScan = 48,
        WaitingForConfirm = 53,
        Canceled = 54,
    }
}