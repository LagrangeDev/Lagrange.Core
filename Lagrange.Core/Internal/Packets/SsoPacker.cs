using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Sign;
using ProtoBuf;
using static Lagrange.Core.Utility.Binary.BinaryPacket;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Internal.Packets;

internal static class SsoPacker
{
    /// <summary>
    /// Build Protocol SSO packet
    /// </summary>
    public static byte[] Build(SsoPacket packet, BotAppInfo appInfo, BotDeviceInfo device, BotKeystore keystore, SignProvider signProvider)
    {
        var writer = new BinaryPacket();
        var signature = signProvider.Sign(device, keystore, packet.Command, packet.Sequence, packet.Payload.ToArray());

        switch (packet.PacketType)
        {
            case 10:
                {
                    writer.Barrier(typeof(uint), () => new BinaryPacket() // Barrier is used to calculate the length of the packet header only
                        .WriteUint(packet.Sequence, false) // sequence
                        .WriteUint(appInfo.SubAppId, false) // appId
                        .WriteUint(appInfo.SubAppId, false) // appId
                        .WriteBytes(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 }, Prefix.None)
                        .WriteBytes(keystore.Session.Tgt, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteString(packet.Command, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(keystore.Session.MsgCookie, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteString(device.System.AndroidId, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(keystore.Session.Ksid, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteString($"|{device.Model.Imsi}|{appInfo.CurrentVersion}", Prefix.Uint16 | Prefix.WithPrefix) // Actually at wtlogin.trans_emp, this string is empty and only prefix 00 02 is given, but we can just simply ignore that situation
                        .WriteBytes(signature, Prefix.Uint32 | Prefix.WithPrefix), false, true); // packet end
                    break;
                }
            case 11:
                {
                    writer.Barrier(typeof(uint), () => new BinaryPacket() // Barrier is used to calculate the length of the packet header only
                        .WriteString(packet.Command, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(keystore.Session.MsgCookie, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(signature, Prefix.Uint32 | Prefix.WithPrefix), false, true);
                    break;
                }
            case 12:
                {
                    writer.Barrier(typeof(uint), () => new BinaryPacket() // Barrier is used to calculate the length of the packet header only
                        .WriteUint(packet.Sequence, false) // sequence
                        .WriteUint(appInfo.SubAppId, false) // appId
                        .WriteUint(2052, false) // LocaleId
                        .WriteBytes(new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, Prefix.None)
                        .WriteBytes(keystore.Session.Tgt, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteString(packet.Command, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(keystore.Session.MsgCookie, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteString(device.System.Guid.ToByteArray().Hex(true), Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteString(appInfo.CurrentVersion, Prefix.Uint16 | Prefix.WithPrefix) // Actually at wtlogin.trans_emp, this string is empty and only prefix 00 02 is given, but we can just simply ignore that situation
                        .WriteBytes(signature, Prefix.Uint32 | Prefix.WithPrefix), false, true);
                    break;
                }
            case 13:
                {
                    var stream = new MemoryStream();
                    var uid = new NTPacketUid { Uid = keystore.Uid };
                    Serializer.Serialize(stream, uid);
                    var uidBytes = keystore.Uid == null ? Array.Empty<byte>() : stream.ToArray();

                    writer.Barrier(typeof(uint), () => new BinaryPacket() // Barrier is used to calculate the length of the packet header only
                        .WriteString(packet.Command, Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix) // TODO: Unknown
                        .WriteBytes(uidBytes, Prefix.Uint32 | Prefix.WithPrefix), false, true);
                    break;
                }
            default:
                throw new Exception($"Unknown packet type: {packet.PacketType}");
        }

        writer.WriteBytes(packet.Payload.ToArray(), Prefix.Uint32 | Prefix.WithPrefix);
        return writer.ToArray();
    }

    /// <summary>
    /// Parse Protocol 10, 11 and 12 SSO packet
    /// </summary>
    public static SsoPacket Parse(uint protocol, byte encode, BinaryPacket packet)
    {
        uint _ = packet.ReadUint(false); // header length
        uint sequence = packet.ReadUint(false);
        int retCode = packet.ReadInt(false);
        string extra = packet.ReadString(Prefix.Uint32 | Prefix.WithPrefix);
        string command = packet.ReadString(Prefix.Uint32 | Prefix.WithPrefix);
        byte[] msgCookie = packet.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix);
        int isCompressed = packet.ReadInt();
        if (protocol == 10)
            packet.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix); // Dummy Sso header
        else
            packet.ReadBytes(Prefix.Uint32 | Prefix.LengthOnly); // Dummy Sso header


        if (retCode == 0) return new SsoPacket(protocol, encode, command, msgCookie, sequence, isCompressed == 0 ? packet : InflatePacket(packet));
        throw new Exception($"Packet '{command}' returns {retCode} with seq: {sequence}, extra: {extra}");
    }

    private static BinaryPacket InflatePacket(BinaryPacket original)
    {
        var raw = original.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix);
        var decompressed = ZCompression.ZDecompress(raw);

        var stream = new MemoryStream();
        stream.Write(BitConverter.GetBytes(decompressed.Length + sizeof(int), false));
        stream.Write(decompressed);
        stream.Position = 0;

        return new BinaryPacket(stream);
    }
}