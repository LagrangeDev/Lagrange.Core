using Microsoft.Extensions.Configuration;

namespace Lagrange.Milky.Configurations;

public class MilkyConfiguration(MilkyHttpServerConfiguration httpServer, string? accessToken = null, MilkyApiConfiguration? api = null, MilkyEventConfiguration? event_ = null)
{
    public string? AccessToken { get; } = accessToken;
    public MilkyHttpServerConfiguration HttpServer { get; } = httpServer;
    public MilkyApiConfiguration Api { get; } = api ?? new();
    [ConfigurationKeyName("Event")]
    private MilkyEventConfiguration Event_ { get; } = event_ ?? new();
    public MilkyEventConfiguration Event => Event_;
}

public class MilkyEventConfiguration(MilkyWebSocketEventConfiguration? webSocket = null, MilkySSEEventConfiguration? sse = null, MilkyWebHookEventConfiguration? webHook = null)
{
    public MilkyWebSocketEventConfiguration? WebSocket { get; } = webSocket;
    public MilkySSEEventConfiguration? SSE { get; } = sse;
    public MilkyWebHookEventConfiguration? WebHook { get; } = webHook;
}

public class MilkyWebHookEventConfiguration(bool enabled, string[] targetUrls)
{
    public bool Enabled { get; } = enabled;
    public string[] TargetUrls { get; } = targetUrls;
}

public class MilkySSEEventConfiguration(bool enabled, string? allowCorsOrigins = null, ulong heartbeatIntervalSeconds = 60)
{
    public bool Enabled { get; } = enabled;
    public string? AllowCorsOrigins { get; } = allowCorsOrigins;
    public ulong HeartbeatIntervalSeconds { get; } = heartbeatIntervalSeconds;
}

public class MilkyWebSocketEventConfiguration(bool enabled)
{
    public bool Enabled { get; init; } = enabled;
}

public class MilkyApiConfiguration
{
}

public class MilkyHttpServerConfiguration(string host, ushort port)
{
    public string Host { get; } = host;
    public ushort Port { get; } = port;
}