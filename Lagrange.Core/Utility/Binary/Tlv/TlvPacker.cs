using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using System.Reflection;

namespace Lagrange.Core.Utility.Binary.Tlv;

internal class TlvPacker
{
    private static readonly Dictionary<ushort, Type> Tlvs = new();
    private static readonly Dictionary<ushort, Type> TlvResps = new();
    private static readonly Dictionary<ushort, Type> TlvQrCodes = new();
    private static readonly Dictionary<ushort, Type> TlvQrCodeResps = new();

    private static readonly Dictionary<Type, TlvEncryptAttribute> TlvEncrypt = new();

    private readonly ServiceInjector _injector;

    static TlvPacker()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var tlvs = assembly.GetTypeByAttributes<TlvAttribute>(out var tlvAttributes);
        for (int i = 0; i < tlvs.Count; i++)
        {
            var attribute = tlvAttributes[i];
            (attribute.IsResponse ? TlvResps : Tlvs)[attribute.TlvCommand] = tlvs[i];
            
            var encrypt = tlvs[i].GetCustomAttribute<TlvEncryptAttribute>();
            if (encrypt != null) TlvEncrypt[tlvs[i]] = encrypt;
        }

        var tlvQrCodes = assembly.GetTypeByAttributes<TlvQrCodeAttribute>(out var tlvQrCodeAttributes);
        for (int i = 0; i < tlvQrCodes.Count; i++)
        {
            var attribute = tlvQrCodeAttributes[i];
            (attribute.IsResponse ? TlvQrCodeResps : TlvQrCodes)[attribute.TlvCommand] = tlvQrCodes[i];
            
            var encrypt = tlvQrCodes[i].GetCustomAttribute<TlvEncryptAttribute>();
            if (encrypt != null) TlvEncrypt[tlvQrCodes[i]] = encrypt;
        }
    }

    public TlvPacker(BotAppInfo appInfo, BotKeystore keystore, BotDeviceInfo deviceInfo)
    {
        _injector = new ServiceInjector();
        
        _injector.AddService(appInfo);
        _injector.AddService(keystore);
        _injector.AddService(deviceInfo);
    }
    
    public TlvPacket Pack(ushort cmd)
    {
        var tlvBody = (TlvBody)_injector.Inject(Tlvs[cmd]);
        var encrypt = ResolveEncryption(Tlvs[cmd]);
        return new TlvPacket(cmd, tlvBody, encrypt);
    }

    public TlvPacket PackQrCode(ushort cmd)
    {
        var tlvBody = (TlvBody)_injector.Inject(TlvQrCodes[cmd]);
        var encrypt = ResolveEncryption(TlvQrCodes[cmd]);
        return new TlvPacket(cmd, tlvBody, encrypt);
    }

    public BinaryPacket Pack(ushort[] cmd)
    {
        var packet = new BinaryPacket().WriteUshort((ushort)cmd.Length);

        foreach (var tlv in cmd) packet.WritePacket(Pack(tlv));
        return packet;
    }

    public BinaryPacket PackQrCode(ushort[] cmd)
    {
        var packet = new BinaryPacket().WriteUshort((ushort)cmd.Length);

        foreach (var tlv in cmd) packet.WritePacket(PackQrCode(tlv));
        return packet;
    }
    
    public static TlvPacket Pack(TlvBody tlv, ushort cmd) => new(cmd, tlv);

    public static TlvBody? ReadTlvBody(ushort cmd, BinaryPacket packet)
    {
        if (!TlvResps.TryGetValue(cmd, out var type))
        {
            if (!Tlvs.TryGetValue(cmd, out type))
            {
                return null;
            }
        }
        
        if (type.GetCustomAttribute<ProtoContractAttribute>() != null)
        {
            using var stream = new MemoryStream(packet.ToArray());
            return (TlvBody)Serializer.Deserialize(type, stream);
        }

        return (TlvBody)packet.Deserialize(type);
    }

    public static TlvBody? ReadTlvBodyQrCode(ushort cmd, BinaryPacket packet)
    {
        if (!TlvQrCodeResps.TryGetValue(cmd, out var type))
        {
            if (!TlvQrCodes.TryGetValue(cmd, out type)) return null;
        }
        
        if (type.GetCustomAttribute<ProtoContractAttribute>() != null)
        {
            using var stream = new MemoryStream(packet.ToArray());
            return (TlvBody)Serializer.Deserialize(type, stream);
        }

        return (TlvBody)packet.Deserialize(type);
    }

    public static Dictionary<ushort, TlvBody> ReadTlvCollections(BinaryPacket payload, bool isQrCode = false)
    {
        ushort tlvCount = payload.ReadUshort();
        var tlvs = new Dictionary<ushort, TlvBody>(tlvCount);

        for (int i = 0; i < tlvCount; i++)
        {            
            ushort cmd = payload.ReadUshort();
            ushort length = payload.ReadUshort();

            var packet = payload.ReadPacket(length);
            var tlvBody = isQrCode ? ReadTlvBodyQrCode(cmd, packet) : ReadTlvBody(cmd, packet);
            if (tlvBody != null) tlvs[cmd] = tlvBody;
        }

        return tlvs;
    }

    private (TeaImpl, byte[])? ResolveEncryption(Type type)
    {
        if (!TlvEncrypt.TryGetValue(type, out var encrypt)) return null;

        var keystore = _injector.GetService<BotKeystore>();
        return encrypt.Type switch
        {
            TlvEncryptAttribute.KeyType.TgtgtKey => (keystore.TeaImpl, keystore.Stub.TgtgtKey),
            TlvEncryptAttribute.KeyType.Password => (keystore.TeaImpl, keystore.Stub.TgtgtKey), // TODO: Implement password encryption
            _ => null
        };
    }
}