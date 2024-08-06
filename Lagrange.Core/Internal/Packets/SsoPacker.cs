using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Generator;
using Lagrange.Core.Utility.Sign;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets;

internal static class SsoPacker
{
    /// <summary>
    /// Build Protocol 12 SSO packet
    /// </summary>
    public static BinaryPacket Build(SsoPacket packet, BotAppInfo appInfo, BotDeviceInfo device, BotKeystore keystore, SignProvider signProvider)
    {
        var writer = new BinaryPacket();

        var sign = signProvider.Sign(packet.Command, packet.Sequence, packet.Payload.ToArray(), out var extra, out var token);
        var signature = new NTDeviceSign
        {
            Sign = sign == null ? null : new SecInfo
            {
                SecSign = sign,
                SecToken = token,
                SecExtra = extra
            },
            Trace = StringGen.GenerateTrace(),
            Uid = keystore.Uid
        };
        var stream = new MemoryStream();
        Serializer.Serialize(stream, signature);
        
        writer.Barrier( w => w// Barrier is used to calculate the length of the packet header only
            .WriteUint(packet.Sequence) // sequence
            .WriteUint((uint)appInfo.SubAppId) // appId
            .WriteUint(2052) // LocaleId
            .WriteBytes("020000000000000000000000".UnHex().AsSpan())
            .WriteBytes(keystore.Session.Tgt, Prefix.Uint32 | Prefix.WithPrefix)
            .WriteString(packet.Command, Prefix.Uint32 | Prefix.WithPrefix)
            .WriteBytes(Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix) // TODO: unknown
            .WriteString(device.Guid.ToByteArray().Hex(true), Prefix.Uint32 | Prefix.WithPrefix)
            .WriteBytes(Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix) // TODO: unknown
            .WriteString(appInfo.CurrentVersion, Prefix.Uint16 | Prefix.WithPrefix) // Actually at wtlogin.trans_emp, this string is empty and only prefix 00 02 is given, but we can just simply ignore that situation
            .WriteBytes(stream.ToArray(), Prefix.Uint32 | Prefix.WithPrefix), Prefix.Uint32 | Prefix.WithPrefix); // packet end
        
        return writer.WriteBytes(packet.Payload.ToArray(), Prefix.Uint32 | Prefix.WithPrefix);
    }

    /// <summary>
    /// Parse Protocol 12 SSO packet
    /// </summary>
    public static SsoPacket Parse(BinaryPacket packet)
    {
        var head = packet.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix);
        var headReader = new BinaryPacket(head);
        
        uint sequence = headReader.ReadUint();
        int retCode = headReader.ReadInt();
        string extra = headReader.ReadString(Prefix.Uint32 | Prefix.WithPrefix);
        string command = headReader.ReadString(Prefix.Uint32 | Prefix.WithPrefix);
        var msgCookie = headReader.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix);
        int isCompressed = headReader.ReadInt();
        var reserveField = headReader.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix);
        
        var body = packet.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix).ToArray();
        var raw = isCompressed switch
        {
            0 or 4 => body,
            1 => ZCompression.ZDecompress(body, false),
            _ => throw new Exception($"Unknown compression type: {isCompressed}")
        };
        
        return retCode == 0 
            ? new SsoPacket(12, command, sequence, raw) 
            : new SsoPacket(12, command, sequence, retCode, extra);
    }
}