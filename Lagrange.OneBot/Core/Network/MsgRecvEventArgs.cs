namespace Lagrange.OneBot.Core.Network;

public class MsgRecvEventArgs(string data, string? identifier = null) : EventArgs
{
    public string? Identifier { get; init; } = identifier;

    public string Data { get; init; } = data;
}
