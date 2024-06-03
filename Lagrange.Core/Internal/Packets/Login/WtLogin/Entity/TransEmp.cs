using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

internal abstract class TransEmp : WtLoginBase
{
    private readonly ushort _qrCodeCommand;
    
    private const string PacketCommand = "wtlogin.trans_emp";
    private const ushort WtLoginCommand = 2066;

    protected TransEmp(ushort qrCmd, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(PacketCommand, WtLoginCommand, keystore, appInfo, device)
    {
        _qrCodeCommand = qrCmd;
    }

    protected override BinaryPacket ConstructData()
    {
        var tlv = ConstructTlv();

        var newPacket =  new BinaryPacket() // length of WriteUshort(43 + tlv.Length + 1) refers to this section
            .WriteByte(2)
            .WriteUshort((ushort)(43 + tlv.Length + 1)) // _head_len = 43 + data.size +1
            .WriteUshort(_qrCodeCommand)
            .WriteBytes(new byte[21])
            .WriteByte(0x03)
            .WriteShort(0x00) // close
            .WriteShort(0x32) // Version Code: 50
            .WriteUint(0) // trans_emp sequence
            .WriteUlong(0) // dummy uin
            .WritePacket(tlv)
            .WriteByte(3);

        var requestBody = new BinaryPacket()
            .WriteUint((uint)DateTimeOffset.Now.ToUnixTimeSeconds())
            .WritePacket(newPacket);

        return new BinaryPacket()
            .WriteByte(0x00) // encryptMethod == EncryptMethod.EM_ST || encryptMethod == EncryptMethod.EM_ECDH_ST
            .WriteUshort((ushort)requestBody.Length)
            .WriteUint((uint)AppInfo.AppId)
            .WriteUint(0x72) // Role
            .WriteBytes(Array.Empty<byte>(), Prefix.Uint16 | Prefix.LengthOnly) // uSt
            .WriteBytes(Array.Empty<byte>(), Prefix.Uint8 | Prefix.LengthOnly) // rollback
            .WritePacket(requestBody);
    }

    public static BinaryPacket DeserializeBody(BotKeystore keystore, BinaryPacket packet, out ushort command)
    {
        packet = DeserializePacket(keystore, packet);
        
        uint packetLength = packet.ReadUint();
        packet.Skip(4); // misc unknown data
        command = packet.ReadUshort();
        packet.Skip(40);
        uint appId = packet.ReadUint();

        return packet;
    }

    protected abstract BinaryPacket ConstructTlv();
}