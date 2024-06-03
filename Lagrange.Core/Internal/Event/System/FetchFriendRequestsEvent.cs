namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendRequestsEvent : ProtocolEvent
{
    private FetchFriendRequestsEvent() : base(true)
    {
    }

    private FetchFriendRequestsEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static FetchFriendRequestsEvent Create() => new();
}