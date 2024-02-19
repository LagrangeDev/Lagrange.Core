namespace Lagrange.OneBot.Core.Network.Options
{
    public sealed class ReverseWSServiceOptions : WSServiceOptions
    {
        public string Suffix { get; set; } = "";

        public uint ReconnectInterval { get; set; } = 5000;
    }
}
