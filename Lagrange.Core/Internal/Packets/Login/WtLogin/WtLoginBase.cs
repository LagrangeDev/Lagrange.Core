using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;

namespace Lagrange.Core.Internal.Packets.Login.WtLogin;

internal abstract class WtLoginBase
{
    protected readonly string Command;
    protected readonly ushort Cmd;
    protected readonly byte CmdVer;
    protected readonly byte PubId;
    protected readonly BotKeystore Keystore;
    protected readonly BotAppInfo AppInfo;
    protected readonly BotDeviceInfo Device;

    protected readonly EcdhImpl EcdhImpl;

    protected WtLoginBase(string command, ushort cmd, byte cmdVar, byte pubId, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
    {
        Command = command;
        Cmd = cmd;
        CmdVer = cmdVar;
        PubId = pubId;
        Keystore = keystore;
        AppInfo = appInfo;
        Device = device;

        switch (AppInfo.Os)
        {
            case "Android":
                EcdhImpl = new EcdhImpl(EcdhImpl.CryptMethod.Prime256V1);
                break;
            case "Linux":
            case "Mac":
            case "Windows":
                EcdhImpl = new EcdhImpl(EcdhImpl.CryptMethod.Secp192K1);
                break;
            default:
                throw new Exception($"Unknown os: {AppInfo.Os}");
        }
    }

    public BinaryPacket ConstructPacket()
    {
        var packet = new BinaryPacket().WriteByte(2); // packet start

        switch (Command)
        {
            case "wtlogin.login":
            case "wtlogin.trans_emp":
                {
                    EcdhImpl.GenerateShared(AppInfo.WtLoginSdk.PubKey, true);
                    var body = ConstructBody();
                    var encrypt = Keystore.TeaImpl.Encrypt(body.ToArray(), EcdhImpl.ShareKey);

                    packet.Barrier(w => 
                    {
                        w.WriteUshort(8001) // ver
                        .WriteUshort(Cmd) // cmd: [wtlogin.login, wtlogin.exchange_emp]: 2064, wtlogin.trans_emp: 2066
                        .WriteUshort(Keystore.Session.Sequence) // unique wtLoginSequence for wtlogin packets only, should be stored in KeyStore
                        .WriteUint(Keystore.Uin) // uin, 0 for wtlogin.trans_emp
                        .WriteByte(3) // extVer
                        .WriteByte(CmdVer) // cmdVer: [wtlogin.trans_emp, wtlogin.login]: 135, wtlogin.exchange_emp: 69
                        .WriteUint(0) // actually unknown const 0
                        .WriteByte(PubId) // pubId (android: 2? other 19)
                        .WriteUshort(0) // insId
                        .WriteUshort(AppInfo.AppClientVersion) // cliType
                        .WriteUint(0); // retryTime
                        if (AppInfo.Os == "Android")
                        {
                            w.WriteByte(2) // const
                                .WriteByte(1) // const
                                .WriteBytes(Keystore.Stub.RandomKey, Prefix.None) // randKey
                                .WriteUshort(0x0131) // android: 0x0131 other: 0
                                .WriteUshort(0x0001) // public_key_ver(custom: 0x0001, default: 0x0002)
                                .WriteBytes(EcdhImpl.GetPublicKey(false), Prefix.Uint16 | Prefix.LengthOnly); // pubKey
                        }
                        else
                        {
                            w.WriteByte(1) // const
                                .WriteByte(1) // const
                                .WriteBytes(Keystore.Stub.RandomKey, Prefix.None) // randKey
                                .WriteUshort(0x0102)
                                .WriteBytes(AppInfo.Os == "Android" ? EcdhImpl.GetPublicKey(false) : EcdhImpl.GetPublicKey(true), Prefix.Uint16 | Prefix.LengthOnly); // pubKey
                        }
                        w.WriteBytes(encrypt, Prefix.None);
                    }, Prefix.Uint16 | Prefix.WithPrefix, 2);
                    break;
                }
            case "wtlogin.exchange_emp":
                {
                    var body = ConstructBody();
                    var encrypt = Keystore.TeaImpl.Encrypt(body.ToArray(), Keystore.Session.WtSessionTicketKey);

                    packet.Barrier(w => w
                        .WriteUshort(8001) // ver
                        .WriteUshort(Cmd) // cmd: [wtlogin.login, wtlogin.exchange_emp]: 2064, wtlogin.trans_emp: 2066
                        .WriteUshort(Keystore.Session.Sequence) // unique wtLoginSequence for wtlogin packets only, should be stored in KeyStore
                        .WriteUint(Keystore.Uin) // uin, 0 for wtlogin.trans_emp
                        .WriteByte(3) // extVer
                        .WriteByte(CmdVer) // cmdVer: [wtlogin.trans_emp, wtlogin.login]: 135, wtlogin.exchange_emp: 69
                        .WriteUint(0) // actually unknown const 0
                        .WriteByte(PubId) // pubId (android: 2? other 19)
                        .WriteUshort(0) // insId
                        .WriteUshort(AppInfo.AppClientVersion) // cliType
                        .WriteUint(0) // retryTime
                        .WriteBytes(Keystore.Session.WtSessionTicket, Prefix.Uint16 | Prefix.LengthOnly) // pubKey
                        .WriteBytes(encrypt, Prefix.None), Prefix.Uint16 | Prefix.WithPrefix, 2);
                    break;
                }
            default: throw new Exception($"Unknown wtlogin cmd: {Command}");
        }

        packet.WriteByte(3); // 0x03 is the packet end
        return packet;
    }

    protected BinaryPacket DeserializePacket(BotKeystore keystore, BinaryPacket packet)
    {
        uint packetLength = packet.ReadUint();
        if (packet.ReadByte() != 0x02) return new BinaryPacket(); // packet header

        ushort internalLength = packet.ReadUshort();
        ushort ver = packet.ReadUshort();
        ushort cmd = packet.ReadUshort();
        ushort sequence = packet.ReadUshort();
        uint uin = packet.ReadUint();
        byte flag = packet.ReadByte();
        byte encrypt_type = packet.ReadByte();
        byte state = packet.ReadByte();
        var encrypted = packet.ReadBytes((int)(packet.Remaining - 1));

        BinaryPacket decrypted;
        switch (encrypt_type)
        {
            case 0:
                {
                    if (state == 180)
                        decrypted = new BinaryPacket(keystore.TeaImpl.Decrypt(encrypted, keystore.Stub.RandomKey));
                    else
                        decrypted = new BinaryPacket(keystore.TeaImpl.Decrypt(encrypted, EcdhImpl.ShareKey));
                    break;
                }
            case 3:
                {
                    if (keystore.Session.WtSessionTicketKey == null) throw new InvalidOperationException("ExchangeKey is null");
                    decrypted = new BinaryPacket(keystore.TeaImpl.Decrypt(encrypted, keystore.Session.WtSessionTicketKey));
                    break;
                }
            case 4:
                {
                    decrypted = new BinaryPacket(keystore.TeaImpl.Decrypt(encrypted, EcdhImpl.ShareKey));

                    var bobPublic = decrypted.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                    EcdhImpl.GenerateShared(bobPublic.ToArray(), true);
                    decrypted = new BinaryPacket(keystore.TeaImpl.Decrypt(decrypted.ReadBytes((int)decrypted.Remaining), EcdhImpl.ShareKey));
                    break;
                }
            default:
                throw new Exception($"Unknown encrypt type: {encrypt_type}");
        }

        if (packet.ReadByte() != 0x03) throw new Exception("Packet end not found"); // packet end

        return decrypted;
    }

    protected abstract BinaryPacket ConstructBody();
}