using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsEvent : ProtocolEvent
{
    public List<BotFriend> Friends { get; } = new();
    
    private FetchFriendsEvent() : base(true)
    {
    }

    private FetchFriendsEvent(int resultCode, List<BotFriend> friends) : base(resultCode)
    {
        Friends = friends;
    }
    
    public static FetchFriendsEvent Create() => new();
    
    public static FetchFriendsEvent Result(int resultCode, List<BotFriend> friends) => new(resultCode, friends);
}