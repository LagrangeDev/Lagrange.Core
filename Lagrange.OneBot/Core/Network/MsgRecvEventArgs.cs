using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core.Network;

public class MsgRecvEventArgs(string data, string? identifier) : EventArgs
{
    public string? Identifier { get; init; } = identifier;

    public string Data { get; init; } = data;
}
