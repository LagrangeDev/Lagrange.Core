using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsRequestsEvent : ProtocolEvent
{
    public List<BotFriendRequest> Requests { get; } = new();

    private FetchFriendsRequestsEvent() : base(true)
    {
    }

    private FetchFriendsRequestsEvent(int resultCode, List<BotFriendRequest> requests) : base(resultCode)
    {
        Requests = requests;
    }
    
    public static FetchFriendsRequestsEvent Create() => new();
    
    public static FetchFriendsRequestsEvent Result(int resultCode, List<BotFriendRequest> requests) => new(resultCode, requests);
}