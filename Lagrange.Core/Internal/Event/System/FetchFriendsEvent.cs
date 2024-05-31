using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsEvent : ProtocolEvent
{
    public List<BotFriend> Friends { get; } = new();
    
    public uint? NextUin { get; }
    
    private FetchFriendsEvent(uint? nextUin) : base(true)
    {
        NextUin = nextUin;
    }

    private FetchFriendsEvent(int resultCode, List<BotFriend> friends, uint? nextUin) : base(resultCode)
    {
        Friends = friends;
        NextUin = nextUin;
    }
    
    public static FetchFriendsEvent Create(uint? nextUin = null) => new(nextUin);
    
    public static FetchFriendsEvent Result(int resultCode, List<BotFriend> friends, uint? nextUin) =>
        new(resultCode, friends, nextUin);
}