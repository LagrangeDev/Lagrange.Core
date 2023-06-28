using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using static Lagrange.Core.Utility.Binary.BinaryPacket;

namespace Lagrange.Core.Core.Packets;

internal static class ServicePacker
{
    public static BinaryPacket BuildProtocol13(BinaryPacket packet, string command, uint sequence)
    {
        var frame = new BinaryPacket();

        frame.Barrier(typeof(uint), () => new BinaryPacket()
            .WriteUint(13, false)
            .WriteByte(0) // flag
            .WriteUint(sequence, false)
            .WriteUint(0, false)
            .WriteUshort(0x530, false)
            .Barrier(typeof(uint), () => new BinaryPacket()
                .WriteString(command, Prefix.Uint32 | Prefix.WithPrefix)
                .WriteBytes(Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix) // TODO: Unknown
                .WriteUint(4, false), false, true)
            .WriteBytes(packet.ToArray(), Prefix.Uint32 | Prefix.WithPrefix), false, true);

        return frame;
    }
    
    /// <summary>
    /// Build Universal Packet, every service should derive from this, protocol 12 only
    /// </summary>
    public static BinaryPacket Build(BinaryPacket packet, BotKeystore keystore)
    {
        var frame = new BinaryPacket();
        
        frame.Barrier(typeof(uint), () => new BinaryPacket()
            .WriteUint(12, false) // protocolVersion
            .WriteByte((byte)(keystore.Session.D2.Length == 0 ? 2 : 1)) // flag
            .WriteBytes(keystore.Session.D2, Prefix.Uint32 | Prefix.WithPrefix)
            .WriteByte(0) // unknown
            .WriteString(keystore.Uin.ToString(), Prefix.Uint32 | Prefix.WithPrefix) // 帅
            .WriteBytes(keystore.TeaImpl.Encrypt(packet.ToArray(), keystore.Session.D2Key).AsSpan()), false, true);
        
        return frame;
    }

    /// <summary>
    /// Parse Universal Packet, every service should derive from this, protocol 12 only
    /// </summary>
    public static BinaryPacket Parse(BinaryPacket packet, BotKeystore keystore)
    {
        uint length = packet.ReadUint(false);
        uint protocol = packet.ReadUint(false);
        byte header = packet.ReadByte();
        uint flag = packet.ReadByte();
        string uin = packet.ReadString(Prefix.Uint32 | Prefix.WithPrefix);

        if (protocol == 13) return new BinaryPacket(); // Only Client.CorrectTime and Heartbeat.Alive so Ignored
        if (protocol != 12) throw new Exception($"Unrecognized protocol: {protocol}");
        if (uin != keystore.Uin.ToString()) throw new Exception($"Uin mismatch: {uin} != {keystore.Uin}");
        
        var encrypted = packet.ReadBytes((int)packet.Remaining);
        var decrypted = keystore.TeaImpl.Decrypt(encrypted, keystore.Session.D2Key);
        
        return new BinaryPacket(decrypted);
    }
}