using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal class Login : WtLoginBase
{
    private const string PacketCommand = "wtlogin.login";

    private const ushort WtLoginCommand = 2064;

    private const ushort InternalCommand = 0x09;

    private static readonly ushort[] ConstructTlvs =
    {
        0x106, 0x144, 0x116, 0x142, 0x145, 0x018, 0x141, 0x177, 0x191, 0x100, 0x107, 0x318, 0x16A, 0x166, 0x521
    };
    
    public Login(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(PacketCommand, WtLoginCommand, keystore, appInfo, device) { }

    protected override BinaryPacket ConstructData() => new BinaryPacket()
        .WriteUshort(InternalCommand)
        .WritePacket(TlvPacker.Pack(ConstructTlvs));
    

    public static Dictionary<ushort, TlvBody> Deserialize(BinaryPacket packet, BotKeystore keystore, out State state)
    {
        packet = DeserializePacket(keystore, packet);
        
        ushort command = packet.ReadUshort();
        if (command != InternalCommand) throw new Exception("Invalid command");
        
        state = (State)packet.ReadByte();
        if (state == State.Success)
        {
            var tlvs = TlvPacker.ReadTlvCollections(packet);
            if (tlvs[0x119] is Tlv119 tlv119)
            {
                var decrypted = keystore.TeaImpl.Decrypt(tlv119.EncryptedTlv, keystore.Stub.TgtgtKey);
                var tlv119Packet = new BinaryPacket(decrypted);
                return TlvPacker.ReadTlvCollections(tlv119Packet);
            }
        }
        else
        {
            return TlvPacker.ReadTlvCollections(packet);
        }

        return new Dictionary<ushort, TlvBody>();
    }

    public enum State : byte
    {
        Success = 0,
        Slider = 2,
        SmsRequired = 160,
    }
}