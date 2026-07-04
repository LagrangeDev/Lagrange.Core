namespace Lagrange.Core.Internal.Events.System;

internal class SetPinGroupEventReq(long groupUin, bool isPin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public bool IsPin { get; } = isPin;
}

internal class SetPinGroupEventResp : ProtocolEvent
{
    public static readonly SetPinGroupEventResp Default = new();
}

internal class SetPinFriendEventReq(string uid, bool isPin) : ProtocolEvent
{
    public string Uid { get; } = uid;

    public bool IsPin { get; } = isPin;
}

internal class SetPinFriendEventResp : ProtocolEvent
{
    public static readonly SetPinFriendEventResp Default = new();
}
