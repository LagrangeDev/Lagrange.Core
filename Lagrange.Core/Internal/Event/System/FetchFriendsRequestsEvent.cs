namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsRequestsEvent : ProtocolEvent
{
    private FetchFriendsRequestsEvent() : base(true)
    {
    }

    private FetchFriendsRequestsEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static FetchFriendsRequestsEvent Create() => new();
}