using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupSetTodoEventReq(long groupUin, ulong sequence) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;
}

internal class GroupSetTodoEventResp : ProtocolEvent
{
    public static readonly GroupSetTodoEventResp Default = new();
}

internal class GroupFinishTodoEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class GroupFinishTodoEventResp : ProtocolEvent
{
    public static readonly GroupFinishTodoEventResp Default = new();
}

internal class GroupRemoveTodoEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class GroupRemoveTodoEventResp : ProtocolEvent
{
    public static readonly GroupRemoveTodoEventResp Default = new();
}
