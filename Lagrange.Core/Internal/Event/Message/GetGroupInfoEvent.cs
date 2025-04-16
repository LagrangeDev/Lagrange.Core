using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Message;

internal class GetGroupInfoEvent : ProtocolEvent
{
    public ulong Uin { get; }

    public string? Message { get; }

    public BotGroupInfo Info { get; }

    protected GetGroupInfoEvent(ulong uin) : base(true)
    {
        Uin = uin;
        Info = new();
    }

    protected GetGroupInfoEvent(int code, string? message, BotGroupInfo info) : base(code)
    {
        Message = message;
        Info = info;
    }

    public static GetGroupInfoEvent Create(ulong uin) => new(uin);

    public static GetGroupInfoEvent Result(int code, string? message, BotGroupInfo info)
    {
        return new(code, message, info);
    }
}