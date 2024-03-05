using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using ProtoBuf.Meta;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x144)]
[TlvEncrypt(TlvEncryptAttribute.KeyType.TgtgtKey)]
internal class Tlv144 : TlvBody
{
    public Tlv144(BotDeviceInfo deviceInfo)
    {
        TlvCount = 5;
        TlvBody = new BinaryPacket()
            .WritePacket(new TlvPacket(0x109, new Tlv109(deviceInfo)))
            .WritePacket(new TlvPacket(0x52D, new Tlv52D(deviceInfo)))
            .WritePacket(new TlvPacket(0x124, new Tlv124(deviceInfo)))
            .WritePacket(new TlvPacket(0x128, new Tlv128(deviceInfo)))
            .WritePacket(new TlvPacket(0x16E, new Tlv16E(deviceInfo)))
            .ToArray();
    }

    [BinaryProperty] public ushort TlvCount { get; set; }

    [BinaryProperty] public byte[] TlvBody { get; set; }
}