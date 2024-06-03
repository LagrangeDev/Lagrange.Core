using Lagrange.Core.Common;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;

namespace Lagrange.Core.Internal.Context;

/// <summary>
/// Log context, all the logs will be dispatched to this context and then to the <see cref="BotLogEvent"/>.
/// </summary>
internal class LogContext : ContextBase
{
    private readonly EventInvoker _invoker;

    public LogContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, EventInvoker invoker)
        : base(collection, keystore, appInfo, device) => _invoker = invoker;
    
    public void LogDebug(string tag, string message) =>
        _invoker.PostEvent(new BotLogEvent(tag, LogLevel.Debug, message));
    
    public void LogVerbose(string tag, string message) => 
        _invoker.PostEvent(new BotLogEvent(tag, LogLevel.Verbose, message));
    
    public void LogInfo(string tag, string message) =>
        _invoker.PostEvent(new BotLogEvent(tag, LogLevel.Information, message));
    
    public void LogWarning(string tag, string message) =>
        _invoker.PostEvent(new BotLogEvent(tag, LogLevel.Warning, message));
    
    public void LogFatal(string tag, string message) =>
        _invoker.PostEvent(new BotLogEvent(tag, LogLevel.Fatal, message));
    
    public void Log(string tag, LogLevel level, string message) =>
        _invoker.PostEvent(new BotLogEvent(tag, level, message));
}