using System.Collections.Concurrent;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Utility.Binary;
using static Lagrange.Core.Utility.Binary.BinaryPacket;

#pragma warning disable CS4014

namespace Lagrange.Core.Internal.Context;

/// <summary>
/// <para>Translate the protocol event into SSOPacket and further ServiceMessage</para>
/// <para>And Dispatch the packet from <see cref="SocketContext"/> by managing the sequence from Tencent's server</para>
/// <para>Every Packet should be send and received from this context instead of being directly send to <see cref="SocketContext"/></para>
/// </summary>
internal class PacketContext : ContextBase
{
    private readonly ConcurrentDictionary<uint, TaskCompletionSource<SsoPacket>> _pendingTasks;

    public PacketContext(ContextCollection collection, BotKeystore Keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(collection, Keystore, appInfo, device)
        => _pendingTasks = new ConcurrentDictionary<uint, TaskCompletionSource<SsoPacket>>();

    private byte[] BuildPacket(BotAppInfo appInfo, SsoPacket packet)
    {
        var sso = SsoPacker.Build(packet, AppInfo, DeviceInfo, Keystore, appInfo.SignProvider);
        var encrypted = (packet.EncodeType) switch
        {
            0 => sso,
            1 => Keystore.TeaImpl.Encrypt(sso, Keystore.Session.D2Key),
            2 => Keystore.TeaImpl.Encrypt(sso, new byte[16]),
            _ => throw new Exception($"Unknown encode type: {packet.EncodeType}")
        };

        var frame = new BinaryPacket();
        switch (packet.PacketType)
        {
            case 10:
                {
                    frame.Barrier(typeof(uint), () => new BinaryPacket()
                        .WriteUint(packet.PacketType, false)
                        .WriteByte(packet.EncodeType)
                        .WriteBytes(packet.EncodeType == 1 ? Keystore.Session.D2 : Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteByte(0) // unknown
                        .WriteString(Keystore.Uin.ToString(), Prefix.Uint32 | Prefix.WithPrefix) // ˧
                        .WriteBytes(encrypted, Prefix.None), false, true);
                    break;
                }
            case 11:
                {
                    frame.Barrier(typeof(uint), () => new BinaryPacket()
                        .WriteUint(packet.PacketType, false)
                        .WriteByte(packet.EncodeType)
                        .WriteUint(packet.Sequence, false)
                        .WriteByte(0)
                        .WriteString(Keystore.Uin.ToString(), Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(encrypted, Prefix.None), false, true);
                    break;
                }
            case 12:
                {
                    frame.Barrier(typeof(uint), () => new BinaryPacket()
                        .WriteUint(packet.PacketType, false)
                        .WriteByte(packet.EncodeType)
                        .WriteBytes(packet.EncodeType == 1 ? Keystore.Session.D2 : Array.Empty<byte>(), Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteByte(0) // unknown
                        .WriteString(Keystore.Uin.ToString(), Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(encrypted, Prefix.None), false, true);
                    break;
                }
            case 13:
                {
                    frame.Barrier(typeof(uint), () => new BinaryPacket()
                        .WriteUint(packet.PacketType, false)
                        .WriteByte(packet.EncodeType)
                        .WriteUint(packet.Sequence, false)
                        .WriteByte(0)
                        .WriteString("0", Prefix.Uint32 | Prefix.WithPrefix)
                        .WriteBytes(encrypted, Prefix.None), false, true);
                    break;
                }
        }
        return frame.ToArray();
    }

    /// <summary>
    /// Send the packet and wait for the corresponding response by the packet's sequence number.
    /// </summary>
    public Task<SsoPacket> SendPacket(BotAppInfo appInfo, SsoPacket packet)
    {
        var task = new TaskCompletionSource<SsoPacket>();
        _pendingTasks.TryAdd(packet.Sequence, task);

        bool _ = Collection.Socket.Send(BuildPacket(appInfo, packet)).GetAwaiter().GetResult();

        return task.Task;
    }

    /// <summary>
    /// Send the packet and don't wait for the corresponding response by the packet's sequence number.
    /// </summary>
    public async Task<bool> PostPacket(BotAppInfo appInfo, SsoPacket packet)
    {
        return await Collection.Socket.Send(BuildPacket(appInfo, packet));
    }

    public void DispatchPacket(BinaryPacket packet)
    {
        uint length = packet.ReadUint(false);
        uint protocol = packet.ReadUint(false);
        byte encode = packet.ReadByte();
        byte flag = packet.ReadByte();
        string uin = packet.ReadString(Prefix.Uint32 | Prefix.WithPrefix);

        if (protocol != 10 && protocol != 11 && protocol != 12 && protocol != 13) throw new Exception($"Unrecognized protocol: {protocol}");
        if (uin != Keystore.Uin.ToString() && protocol == 12) throw new Exception($"Uin mismatch: {uin} != {Keystore.Uin}");

        var encrypted = packet.ReadBytes((int)packet.Remaining);
        var decrypted = (encode) switch
        {
            0 => encrypted,
            1 => Keystore.TeaImpl.Decrypt(encrypted, Keystore.Session.D2Key),
            2 => Keystore.TeaImpl.Decrypt(encrypted, new byte[16]),
            _ => throw new Exception($"Unknown encode type: {encode}")
        };
        if (decrypted == null) return;
        var service = new BinaryPacket(decrypted);

        var sso = SsoPacker.Parse(protocol, encode, service);

        Keystore.Session.MsgCookie = sso.MsgCookie;

        if (_pendingTasks.TryRemove(sso.Sequence, out var task)) task.SetResult(sso);
        else Collection.Business.HandleServerPacket(sso);
    }
}