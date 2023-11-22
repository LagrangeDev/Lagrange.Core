namespace Lagrange.OneBot.Core.Network.Options
{
    public sealed class ReverseWSServiceOptions : WSServiceOptions
    {
        public string Suffix { get; set; }

        public ReverseWSServiceOptions(string host, uint port, uint heartBeatInterval, string suffix, string? accessToken) : base(host, port, heartBeatInterval, accessToken)
        {
            Suffix = suffix;
        }
    }
}
