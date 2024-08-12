namespace Lagrange.OneBot.Core.Network.Options;

public sealed class ReverseWSServiceOptions : WSServiceOptions
{
    public string Suffix { get; set; } = "";

    public string ApiSuffix { get; set; } = "";

    public string EventSuffix { get; set; } = "";

    public bool UseUniversalClient { get; set; } = true;
    
    public bool IgnoreSslCertificate { get; set; } = false;

    public uint ReconnectInterval { get; set; } = 5000;
}