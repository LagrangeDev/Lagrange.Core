using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Core.Packets.Login.WtLogin;

internal abstract class WtLoginBase
{
    protected readonly string Command;
    protected readonly ushort Cmd;
    protected readonly BotKeystore Keystore;
    protected readonly BotAppInfo AppInfo;
    protected readonly BotDeviceInfo Device;

    protected readonly TlvPacker TlvPacker;
    
    protected WtLoginBase(string command, ushort cmd, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
    {
        Command = command;
        Cmd = cmd;
        Keystore = keystore;
        AppInfo = appInfo;
        Device = device;
        TlvPacker = new TlvPacker(appInfo, keystore, device);
    }
    
    public BinaryPacket ConstructPacket()
    {
        var body = ConstructBody();
        var encrypt = Keystore.SecpImpl.Encrypt(body.ToArray());
        
        var packet = new BinaryPacket().WriteByte(2); // packet start
        
        packet.Barrier(typeof(ushort), () => new BinaryPacket()
            .WriteUshort(8001, false) // ver
            .WriteUshort(Cmd, false) // cmd: wtlogin.trans_emp: 2066, wtlogin.login: 2064
            .WriteUshort(Keystore.Session.Sequence, false) // unique wtLoginSequence for wtlogin packets only, should be stored in KeyStore
            .WriteUint(Keystore.Uin, false) // uin, 0 for wtlogin.trans_emp
            .WriteByte(3) // extVer
            .WriteByte(135) // cmdVer
            .WriteUint(0, false) // actually unknown const 0
            .WriteByte(19) // pubId
            .WriteUshort(0, false) // insId
            .WriteUshort(AppInfo.AppClientVersion, false) // cliType
            .WriteUint(0, false) // retryTime
            .WriteByte(1) // const
            .WriteByte(1) // const
            .WriteBytes(Keystore.Stub.RandomKey.AsSpan()) // randKey
            .WriteUshort(0x102, false) // unknown const, 腾讯你妈妈死啦
            .WriteBytes(Keystore.SecpImpl.GetPublicKey(), BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly) // pubKey
            .WriteBytes(encrypt.AsSpan())
            .WriteByte(3), false, true, 1); // 0x03 is the packet end
        
        return packet;
    }

    protected static BinaryPacket DeserializePacket(BotKeystore keystore, BinaryPacket packet)
    {
        uint packetLength = packet.ReadUint(false);
        if (packet.ReadByte() != 0x02) return new BinaryPacket(); // packet header
        
        ushort internalLength = packet.ReadUshort(false);
        ushort ver = packet.ReadUshort(false);
        ushort cmd = packet.ReadUshort(false);
        ushort sequence = packet.ReadUshort(false);
        uint uin = packet.ReadUint(false);
        byte flag = packet.ReadByte();
        ushort retryTime = packet.ReadUshort(false);

        var encrypted = packet.ReadBytes((int)(packet.Remaining - 1));
        var decrypted = new BinaryPacket(keystore.SecpImpl.Decrypt(encrypted));
        if (packet.ReadByte() != 0x03) throw new Exception("Packet end not found"); // packet end

        return decrypted;
    }

    protected abstract BinaryPacket ConstructBody();
}