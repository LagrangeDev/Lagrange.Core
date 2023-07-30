using System.Collections.Concurrent;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Network;
using Lagrange.Core.Core.Packets;

namespace Lagrange.Core.Core.Context;

/// <summary>
/// <para>Provides BDH (Big-Data Highway) Operation</para>
/// </summary>
internal class HighwayContext : ContextBase
{
    private readonly ConcurrentDictionary<uint, TaskCompletionSource<SsoPacket>> _uploadingTasks;
    
    public HighwayContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(collection, keystore, appInfo, device)
    {
        _uploadingTasks = new ConcurrentDictionary<uint, TaskCompletionSource<SsoPacket>>();
    }
}