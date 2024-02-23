namespace Lagrange.OneBot.Core.Network.Options;

public sealed class ReverseHttpServiceOptions : HttpServiceOptions
{
    public string Suffix { get; set; } = "";

    public uint HeartBeatInterval { get; set; } = 5000;
}
