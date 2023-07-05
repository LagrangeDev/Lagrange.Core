using System.Collections.Concurrent;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Utility.Binary;
#pragma warning disable CS4014

namespace Lagrange.Core.Core.Context;

/// <summary>
/// <para>Translate the protocol event into SSOPacket and further ServiceMessage</para>
/// <para>And Dispatch the packet from <see cref="SocketContext"/> by managing the sequence from Tencent's server</para>
/// <para>Every Packet should be send and received from this context instead of being directly send to <see cref="SocketContext"/></para>
/// </summary>
internal class PacketContext : ContextBase
{
    private readonly ConcurrentDictionary<uint, TaskCompletionSource<SsoPacket>> _pendingTasks;
    
    public PacketContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(collection, keystore, appInfo, device)
    {
        _pendingTasks = new ConcurrentDictionary<uint, TaskCompletionSource<SsoPacket>>();
    }
    
    /// <summary>
    /// Send the packet and wait for the corresponding response by the packet's sequence number.
    /// </summary>
    public Task<SsoPacket> SendPacket(SsoPacket packet)
    {
        var task = new TaskCompletionSource<SsoPacket>();
        _pendingTasks.TryAdd(packet.Sequence, task);

        switch (packet.PacketType)
        {
            case 12:
            {
                var sso = SsoPacker.Build(packet, AppInfo, DeviceInfo, Keystore);
                var service = ServicePacker.BuildProtocol12(sso, Keystore);
                bool _ = Collection.Socket.Send(service.ToArray()).Result;
                break;
            }
            case 13:
            {
                var service = ServicePacker.BuildProtocol13(packet.Payload, Keystore, packet.Command, packet.Sequence);
                bool _ = Collection.Socket.Send(service.ToArray()).Result;
                break;
            }
        }

        return task.Task;
    }
    
    /// <summary>
    /// Send the packet and don't wait for the corresponding response by the packet's sequence number.
    /// </summary>
    public async Task<bool> PostPacket(SsoPacket packet)
    {
        switch (packet.PacketType)
        {
            case 12:
            {
                var sso = SsoPacker.Build(packet, AppInfo, DeviceInfo, Keystore);
                var service = ServicePacker.BuildProtocol12(sso, Keystore);
                return await Collection.Socket.Send(service.ToArray());
            }
            case 13:
            {
                var service = ServicePacker.BuildProtocol13(packet.Payload, Keystore, packet.Command, packet.Sequence);
                return await Collection.Socket.Send(service.ToArray());
            }
            default:
                return false;
        }
    }

    /// <summary>
    /// Handle the incoming packet with new sequence number.
    /// </summary>
    public async Task<bool> HandleServerPacket(SsoPacket packet)
    {
        bool success = false;
        
        var events = Collection.Service.ResolveEventByPacket(packet);
        foreach (var @event in events)
        {
            var isSuccessful = await Collection.Business.HandleIncomingEvent(@event);
            if (!isSuccessful) break;
            
            success = true;
        }

        return success;
    }
    
    public void DispatchPacket(BinaryPacket packet)
    {
        var service = ServicePacker.Parse(packet, Keystore);
        if (service.Length == 0) return;

        var sso = SsoPacker.Parse(service);
        
        if (_pendingTasks.TryRemove(sso.Sequence, out var task)) task.SetResult(sso);
        else HandleServerPacket(sso);
    }
}