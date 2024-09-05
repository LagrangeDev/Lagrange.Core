namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsAndFriendGroupsRequestsEvent : ProtocolEvent
{
    private FetchFriendsAndFriendGroupsRequestsEvent() : base(true)
    {
    }

    private FetchFriendsAndFriendGroupsRequestsEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static FetchFriendsAndFriendGroupsRequestsEvent Create() => new();
}