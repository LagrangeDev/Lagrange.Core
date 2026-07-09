using System.Text.Json.Serialization;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Core.Services;

namespace Lagrange.Core.Common;

/// <summary>
/// Configuration for The bot client
/// </summary>
[Serializable]
public class BotConfig
{
    /// <summary>
    /// The protocol for the client, default is Linux
    /// </summary>
    public Protocols Protocol { get; set; } = Protocols.Linux;
    
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    /// <summary>
    /// Auto reconnect to server when disconnected
    /// </summary>
    public bool AutoReconnect { get; set; } = true;
    
    /// <summary>
    /// Use the IPv6 to connect to server, only if your network support IPv6
    /// </summary>
    public bool UseIPv6Network { get; set; } = false;
    
    /// <summary>
    /// Get optimum server from Tencent MSF server, set to false to use hardcode server
    /// </summary>
    public bool GetOptimumServer { get; set; } = true;

    /// <summary>
    /// The maximum size of the highway block in byte, max 1MB (1024 * 1024 byte)
    /// </summary>
    public uint HighwayChunkSize { get; set; } = 1024 * 1024;
    
    /// <summary>
    /// Highway Uploading Concurrency, if the image failed to send, set this to 1
    /// </summary>
    public uint HighwayConcurrent { get; set; } = 4;
    

    /// <summary>
    /// Refresh the session when the session is about to expire
    /// </summary>
    public bool AutoReLogin { get; set; } = true;

    /// <summary>
    /// The Sign Provider for the bot, if null, the bot will use the default sign provider
    /// </summary>
    public BotSignProvider? SignProvider { get; set; }

    /// <summary>
    /// Custom protocol services loaded when the bot context is created.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<IService> CustomServices { get; init; } = [];
}

/// <summary>
/// The Protocol for the client
/// </summary>
[Flags]
public enum Protocols : byte
{
    None         = 0b00000000,
    
    Windows      = 0b00000001,
    MacOs        = 0b00000010,
    Linux        = 0b00000100,
    AndroidPhone = 0b00001000,
    AndroidPad   = 0b00010000,
    AndroidWatch = 0b00100000,
    
    PC           = Windows | MacOs | Linux,
    Android      = AndroidPhone | AndroidPad | AndroidWatch,
    All          = Windows | MacOs | Linux | AndroidPhone | AndroidPad | AndroidWatch,
}
