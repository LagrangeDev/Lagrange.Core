namespace Lagrange.OneBot.Core.Network;

public class MsgRecvEventArgs(string data) : EventArgs
{
    public string Data { get; init; } = data;
}
