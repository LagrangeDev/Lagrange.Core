using Microsoft.Extensions.Configuration;

namespace Lagrange.Milky.Configurations;

public class MilkyConfiguration(string? accessToken = null, MilkyHttpConfiguration? http = null)
{
    public string? AccessToken { get; } = accessToken;

    public MilkyHttpConfiguration? Http { get; } = http;
}

public class MilkyHttpConfiguration(string host, ushort port, MilkyHttpApiConfiguration? api = null, MilkyWSConfiguration? ws = null, MilkySSEConfiguration? sse = null)
{
    public string Host { get; } = host;
    public ushort Port { get; } = port;

    public MilkyHttpApiConfiguration? Api { get; } = api;
    public MilkyWSConfiguration? WS { get; } = ws;
    public MilkySSEConfiguration? SSE { get; } = sse;
}

public class MilkyHttpApiConfiguration(bool enabled, string path = "/api")
{
    public bool Enabled { get; } = enabled;
    public string Path { get; } = path;
}

public class MilkyWSConfiguration(MilkyWSEventConfiguration? event_)
{
    [ConfigurationKeyName("Event")]
    private MilkyWSEventConfiguration? Event_ { get; } = event_; // dotnet/runtime#130433
    public MilkyWSEventConfiguration? Event => Event_;
}

public class MilkyWSEventConfiguration(bool enabled, string path = "/event")
{
    public bool Enabled { get; } = enabled;
    public string Path { get; } = path;
}

public class MilkySSEConfiguration(MilkySSEEventConfiguration? event_)
{
    [ConfigurationKeyName("Event")]
    public MilkySSEEventConfiguration? Event_ { get; } = event_; // dotnet/runtime#130433
    public MilkySSEEventConfiguration? Event => Event_;
}

public class MilkySSEEventConfiguration(bool enabled, string path = "/event")
{
    public bool Enabled { get; } = enabled;
    public string Path { get; } = path;
}