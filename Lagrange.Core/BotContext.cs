using Lagrange.Core.Common;
using Lagrange.Core.Internal.Context;
using Lagrange.Core.Internal.Event;

namespace Lagrange.Core;

public class BotContext : IDisposable
{
    public readonly EventInvoker Invoker;

    internal readonly Utility.TaskScheduler Scheduler;
    
    internal readonly ContextCollection ContextCollection;

    private readonly BotAppInfo _appInfo;
    
    private readonly BotConfig _config;
    
    private readonly BotDeviceInfo _deviceInfo;
    
    private readonly BotKeystore _keystore;
    
    internal BotContext(BotConfig config, BotDeviceInfo deviceInfo, BotKeystore keystore)
    {
        Invoker = new EventInvoker(this);
        Scheduler = new Utility.TaskScheduler();
        
        _config = config;
        _appInfo = BotAppInfo.ProtocolToAppInfo[config.Protocol];
        _deviceInfo = deviceInfo;
        _keystore = keystore;
        
        ContextCollection = new ContextCollection(_keystore, _appInfo, _deviceInfo, _config, Invoker, Scheduler);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Invoker.Dispose();
    }
}