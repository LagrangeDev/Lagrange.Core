using Lagrange.Core.Common;
using Lagrange.Core.Event;
using Lagrange.Core.Internal.Context;

namespace Lagrange.Core;

public class BotContext : IDisposable
{
    public readonly EventInvoker Invoker;

    public uint BotUin => ContextCollection.Keystore.Uin;

    public string? BotName => ContextCollection.Keystore.Info?.Name;

    internal readonly Utility.TaskScheduler Scheduler;

    internal readonly ContextCollection ContextCollection;

    public BotAppInfo AppInfo { get; }

    public BotConfig Config { get; }

    private readonly BotDeviceInfo _deviceInfo;

    private readonly BotKeystore _keystore;

    internal BotContext(BotConfig config, BotDeviceInfo deviceInfo, BotKeystore keystore, BotAppInfo appInfo)
    {
        Invoker = new EventInvoker(this);
        Scheduler = new Utility.TaskScheduler();

        Config = config;
        AppInfo = appInfo;
        _deviceInfo = deviceInfo;
        _keystore = keystore;

        ContextCollection = new ContextCollection(_keystore, AppInfo, _deviceInfo, Config, Invoker, Scheduler);
    }

    public void Dispose()
    {
        ContextCollection.Dispose();
        GC.SuppressFinalize(this);
    }
}