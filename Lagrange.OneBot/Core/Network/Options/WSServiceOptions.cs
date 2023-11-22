namespace Lagrange.OneBot.Core.Network.Options
{
    public abstract class WSServiceOptions
    {
        public string Host { get; set; }

        public uint Port { get; set; }

        public uint HeartBeatInterval { get; set; }

        public string? AccessToken { get; set; }

        protected WSServiceOptions(string host, uint port, uint heartBeatInterval, string? accessToken)
        {
            Host = host;
            Port = port;
            HeartBeatInterval = heartBeatInterval;
            AccessToken = accessToken;
        }
    }
}
