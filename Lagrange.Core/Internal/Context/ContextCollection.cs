using Lagrange.Core.Common;
using Lagrange.Core.Event;
using TaskScheduler = Lagrange.Core.Utility.TaskScheduler;

namespace Lagrange.Core.Internal.Context;

internal class ContextCollection : IDisposable
{
    public PacketContext Packet { get; }
    public SocketContext Socket { get; }
    public ServiceContext Service { get; }
    public BusinessContext Business { get; }
    public LogContext Log { get; }
    public HighwayContext Highway { get; }
    
    public BotKeystore Keystore { get; }
    public BotAppInfo AppInfo { get; }
    public BotDeviceInfo Device { get; }
    
    public TaskScheduler Scheduler { get; }
    public EventInvoker Invoker { get; }
    
    public ContextCollection(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, BotConfig config,
        EventInvoker invoker, TaskScheduler scheduler)
    {
        Log = new LogContext(this, keystore, appInfo, device, invoker);
        Packet = new PacketContext(this, keystore, appInfo, device, config);
        Socket = new SocketContext(this, keystore, appInfo, device, config);
        Service = new ServiceContext(this, keystore, appInfo, device);
        Business = new BusinessContext(this, keystore, appInfo, device);
        Highway = new HighwayContext(this, keystore, appInfo, device, config);

        Keystore = keystore;
        AppInfo = appInfo;
        Device = device;

        Scheduler = scheduler;
        Invoker = invoker;
    }

    public void Dispose()
    {
        Socket.Dispose();
        Highway.Dispose();
        Invoker.Dispose();
        Scheduler.Dispose();
    }
}