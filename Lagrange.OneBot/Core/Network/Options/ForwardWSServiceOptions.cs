namespace Lagrange.OneBot.Core.Network.Options
{
    public class ForwardWSServiceOptions : WSServiceOptions
    {
        public ForwardWSServiceOptions(string host, uint port, uint heartBeatInterval, string? accessToken) : base(host, port, heartBeatInterval, accessToken)
        {
            
        }
    }
}
