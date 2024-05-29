using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin;

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
        var body = ConstructData();
        var encrypt = Keystore.SecpImpl.Encrypt(body.ToArray());
        
        var packet = new BinaryPacket()
            .WriteByte(2) // packet start
            .Barrier(w => w
                .WriteUshort(8001) // ver
                .WriteUshort(Cmd) // cmd: wtlogin.trans_emp: 2066, wtlogin.login: 2064
                .WriteUshort(Keystore.Session.Sequence) // unique wtLoginSequence for wtlogin packets only, should be stored in KeyStore
                .WriteUint(Keystore.Uin) // uin, 0 for wtlogin.trans_emp
                .WriteByte(3) // extVer
                .WriteByte(135) // cmdVer
                .WriteUint(0) // actually unknown const 0
                .WriteByte(19) // pubId
                .WriteUshort(0) // insId
                .WriteUshort(AppInfo.AppClientVersion) // cliType
                .WriteUint(0) // retryTime
                .WritePacket(BuildEncryptHead())
                .WriteBytes(encrypt.AsSpan())
                .WriteByte(3), Prefix.Uint16 | Prefix.WithPrefix, 1); // 0x03 is the packet end
        
        // for the addition of 1, the packet start should be counted in
        
        return packet;
    }

    protected static BinaryPacket DeserializePacket(BotKeystore keystore, BinaryPacket packet)
    {
        uint packetLength = packet.ReadUint();
        if (packet.ReadByte() != 0x02) return new BinaryPacket(); // packet header
        
        ushort internalLength = packet.ReadUshort();
        ushort ver = packet.ReadUshort();
        ushort cmd = packet.ReadUshort();
        ushort sequence = packet.ReadUshort();
        uint uin = packet.ReadUint();
        byte flag = packet.ReadByte();
        ushort retryTime = packet.ReadUshort();

        var encrypted = packet.ReadBytes((int)(packet.Remaining - 1));
        var decrypted = new BinaryPacket(keystore.SecpImpl.Decrypt(encrypted));
        if (packet.ReadByte() != 0x03) throw new Exception("Packet end not found"); // packet end

        return decrypted;
    }

    protected abstract BinaryPacket ConstructData();

    private BinaryPacket BuildEncryptHead() => new BinaryPacket()
        .WriteByte(1) // const
        .WriteByte(1) // const
        .WriteBytes(Keystore.Stub.RandomKey.AsSpan()) // randKey
        .WriteUshort(0x102) // unknown const, 腾讯你妈妈死啦
        .WriteBytes(Keystore.SecpImpl.GetPublicKey(), Prefix.Uint16 | Prefix.LengthOnly); // pubKey
}