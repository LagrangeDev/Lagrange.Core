using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class TransEmp0x0012 : TransEmp
{
    private const ushort QrCodeCommand = 0x0012;
    
    public TransEmp0x0012(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(QrCodeCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructTransEmp()
    {
        if (Keystore.Session.QrSign != null)
        {
            return new BinaryPacket()
                .WriteBytes(Keystore.Session.QrSign, BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly)
                .WriteUshort(0, false)
                .WriteUlong(0, false)
                .WriteByte(0)
                .WriteUshort(0);
        }

        throw new Exception("QrSign is null");
    }

    public static Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, out State qrState)
    {
        if ((qrState = (State)packet.ReadByte()) == State.Confirmed)
        {
            packet.Skip(12); // misc unknown data
            return TlvPacker.ReadTlvCollections(packet);
        }

        return new Dictionary<ushort, TlvBody>();
    }
}