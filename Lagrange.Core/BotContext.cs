using System.Diagnostics.CodeAnalysis;
using Lagrange.Core.Common;
using Lagrange.Core.Events;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Core.Internal.Context;

namespace Lagrange.Core;

public class BotContext : IDisposable
{
    internal BotContext(BotConfig config, BotKeystore keystore, BotAppInfo appInfo)
    {
        Config = config;
        AppInfo = appInfo;
        Keystore = keystore;
        
        EventInvoker = new EventInvoker(this);
        
        CacheContext = new CacheContext(this);
        PacketContext = new PacketContext(this);
        ServiceContext = new ServiceContext(this);
        SocketContext = new SocketContext(this);
        EventContext = new EventContext(this);
        HighwayContext = new HighwayContext(this);
        FlashTransferContext = new FlashTransferContext(this);
    }

    public BotConfig Config { get; }
    public BotAppInfo AppInfo { get; }
    public BotKeystore Keystore { get; }
    public long BotUin => Keystore.Uin;
    public BotInfo? BotInfo => Keystore.BotInfo;
    
    public bool IsOnline { get; internal set; }
    public EventInvoker EventInvoker { get; }

    /// <summary>
    /// Sends a protocol event and parses its typed response.
    /// </summary>
    public ValueTask<TResponse> SendEvent<TResponse>(ProtocolEvent request) where TResponse : ProtocolEvent =>
        EventContext.SendEvent<TResponse>(request);
    
    internal CacheContext CacheContext { get; }
    internal PacketContext PacketContext { get; }
    internal ServiceContext ServiceContext { get; }
    internal SocketContext SocketContext { get; }
    internal EventContext EventContext { get; }
    
    internal HighwayContext HighwayContext { get; }
    internal FlashTransferContext FlashTransferContext { get; }

    #region Shortcut Methods
    
    public void LogCritical(string tag, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, Exception? exception = null, params object?[] args)
    {
        EventInvoker.PostEvent(new BotLogEvent(tag, LogLevel.Critical, string.Format(text, args), exception));
    }

    public void LogError(string tag, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, Exception? exception = null, params object?[] args)
    {
        if (Config.LogLevel > LogLevel.Error) return;
        
        EventInvoker.PostEvent(new BotLogEvent(tag, LogLevel.Error, string.Format(text, args), exception));
    }

    public void LogWarning(string tag, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, Exception? exception = null, params object?[] args)
    {
        if (Config.LogLevel > LogLevel.Warning) return;
        
        EventInvoker.PostEvent(new BotLogEvent(tag, LogLevel.Warning, string.Format(text, args), exception));
    }

    public void LogInfo(string tag, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args)
    {
        if (Config.LogLevel > LogLevel.Information) return;
        
        EventInvoker.PostEvent(new BotLogEvent(tag, LogLevel.Information, string.Format(text, args), null));
    }

    public void LogDebug(string tag, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args)
    {
        if (Config.LogLevel > LogLevel.Debug) return;
        
        EventInvoker.PostEvent(new BotLogEvent(tag, LogLevel.Debug, string.Format(text, args), null));
    }

    public void LogTrace(string tag, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args)
    {
        if (Config.LogLevel > LogLevel.Trace) return;
        
        EventInvoker.PostEvent(new BotLogEvent(tag, LogLevel.Trace, string.Format(text, args), null));
    }

    #endregion

    public void Dispose()
    {
        EventInvoker.Dispose();
        SocketContext.Dispose();
        EventContext.Dispose();
    }
}
