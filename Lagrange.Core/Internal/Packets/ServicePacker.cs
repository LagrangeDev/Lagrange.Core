using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;
using static Lagrange.Core.Utility.Binary.BinaryPacket;

namespace Lagrange.Core.Internal.Packets;

internal static class ServicePacker
{
    public static BinaryPacket BuildProtocol13(BinaryPacket packet, BotKeystore keystore, string command, uint sequence)
    {
        var frame = new BinaryPacket();
        
        using var stream = new MemoryStream();
        var uid = new NTPacketUid { Uid = keystore.Uid };
        Serializer.Serialize(stream, uid);
        var uidBytes = keystore.Uid == null ? Array.Empty<byte>() : stream.ToArray();

        frame.Barrier(w => w
            .WriteUint(13)
            .WriteByte(0) // flag
            .WriteUint(sequence)
            .WriteByte(0)
            .WriteString("0", Prefix.Uint32 | Prefix.WithPrefix)
            .Barrier(w => w
                .WriteString(command, Prefix.Uint32 | Prefix.WithPrefix)
                .WriteBytes(Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix) // TODO: Unknown
                .WriteBytes(uidBytes, Prefix.Uint32 | Prefix.WithPrefix), Prefix.Uint32 | Prefix.WithPrefix)
            .WriteBytes(packet.ToArray(), Prefix.Uint32 | Prefix.WithPrefix), Prefix.Uint32 | Prefix.WithPrefix);

        return frame;
    }
    
    /// <summary>
    /// Build Universal Packet, every service should derive from this, protocol 12 only
    /// </summary>
    public static BinaryPacket BuildProtocol12(BinaryPacket packet, BotKeystore keystore)
    {
        var frame = new BinaryPacket();
        
        frame.Barrier(w => w
            .WriteUint(12) // protocolVersion
            .WriteByte((byte)(keystore.Session.D2.Length == 0 ? 2 : 1)) // flag
            .WriteBytes(keystore.Session.D2, Prefix.Uint32 | Prefix.WithPrefix)
            .WriteByte(0) // unknown
            .WriteString(keystore.Uin.ToString(), Prefix.Uint32 | Prefix.WithPrefix) // 帅
            .WriteBytes(keystore.TeaImpl.Encrypt(packet.ToArray(), keystore.Session.D2Key).AsSpan()), Prefix.Uint32 | Prefix.WithPrefix);
        
        return frame;
    }

    /// <summary>
    /// Parse Universal Packet, every service should derive from this, protocol 12 and 13
    /// </summary>
    public static BinaryPacket Parse(BinaryPacket packet, BotKeystore keystore)
    {
        uint length = packet.ReadUint();
        uint protocol = packet.ReadUint();
        byte authFlag = packet.ReadByte();
        byte flag = packet.ReadByte();
        string uin = packet.ReadString(Prefix.Uint32 | Prefix.WithPrefix);

        if (protocol != 12 && protocol != 13) throw new Exception($"Unrecognized protocol: {protocol}");
        if (uin != keystore.Uin.ToString() && protocol == 12) throw new Exception($"Uin mismatch: {uin} != {keystore.Uin}");
        
        var encrypted = packet.ReadBytes((int)packet.Remaining);
        var decrypted = authFlag switch
        {
            0 => encrypted,
            1 => keystore.TeaImpl.Decrypt(encrypted, keystore.Session.D2Key),
            2 => keystore.TeaImpl.Decrypt(encrypted, new byte[16]),
            _ => throw new Exception($"Unrecognized auth flag: {authFlag}")
        };
        
        return new BinaryPacket(decrypted);
    }
}