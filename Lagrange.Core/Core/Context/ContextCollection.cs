using Lagrange.Core.Common;
using Lagrange.Core.Core.Event;
using TaskScheduler = Lagrange.Core.Utility.TaskScheduler;

namespace Lagrange.Core.Core.Context;

internal class ContextCollection
{
    public PacketContext Packet { get; }
    public SocketContext Socket { get; }
    public ServiceContext Service { get; }
    public BusinessContext Business { get; }
    public LogContext Log { get; }
    
    public BotKeystore Keystore { get; }
    public BotAppInfo AppInfo { get; }
    public BotDeviceInfo Device { get; }
    
    public TaskScheduler Scheduler { get; }
    public EventInvoker Invoker { get; }
    
    public ContextCollection(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, EventInvoker invoker, 
        TaskScheduler scheduler)
    {
        Packet = new PacketContext(this, keystore, appInfo, device);
        Socket = new SocketContext(this, keystore, appInfo, device);
        Service = new ServiceContext(this, keystore, appInfo, device);
        Business = new BusinessContext(this, keystore, appInfo, device);
        Log = new LogContext(this, keystore, appInfo, device, invoker);

        Keystore = keystore;
        AppInfo = appInfo;
        Device = device;

        Scheduler = scheduler;
        Invoker = invoker;
    }
}