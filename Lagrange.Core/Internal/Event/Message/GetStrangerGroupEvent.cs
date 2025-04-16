using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Message;

internal class GetStrangerGroupInfoEvent : ProtocolEvent
{
    public ulong Uin { get; }

    public string? Message { get; }

    public BotStrangerGroupInfo Info { get; }

    protected GetStrangerGroupInfoEvent(ulong uin) : base(true)
    {
        Uin = uin;
        Info = new();
    }

    protected GetStrangerGroupInfoEvent(int code, string? message, BotStrangerGroupInfo info) : base(code)
    {
        Message = message;
        Info = info;
    }

    public static GetStrangerGroupInfoEvent Create(ulong uin) => new(uin);

    public static GetStrangerGroupInfoEvent Result(int code, string? message, BotStrangerGroupInfo info)
    {
        return new(code, message, info);
    }
}