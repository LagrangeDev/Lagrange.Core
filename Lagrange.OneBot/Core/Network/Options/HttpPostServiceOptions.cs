namespace Lagrange.OneBot.Core.Network.Options;

public sealed class HttpPostServiceOptions : HttpServiceOptions
{
    public string Suffix { get; set; } = "";

    public uint HeartBeatInterval { get; set; } = 5000;

    public bool HeartBeatEnable { get; set; } = true;

    public string Secret { get; set; } = string.Empty;
}
