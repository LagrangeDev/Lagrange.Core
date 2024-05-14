namespace Lagrange.OneBot.Core.Network.Options;

public abstract class WSServiceOptions
{
    public string Host { get; set; } = "";

    public uint Port { get; set; }

    public uint HeartBeatInterval { get; set; } = 5000; // by default 5000

    public bool HeartBeatEnable { get; set; } = true;

    public string? AccessToken { get; set; }
}