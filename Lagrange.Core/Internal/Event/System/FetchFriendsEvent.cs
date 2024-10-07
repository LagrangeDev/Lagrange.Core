using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsEvent : ProtocolEvent
{
    public List<BotFriend> Friends { get; } = new();

    public Dictionary<uint, string> FriendGroups { get; } = new();

    public uint? NextUin { get; }

    private FetchFriendsEvent(uint? nextUin) : base(true)
    {
        NextUin = nextUin;
    }

    private FetchFriendsEvent(int resultCode, List<BotFriend> friends, Dictionary<uint, string> friendGroups, uint? nextUin) : base(resultCode)
    {
        Friends = friends;
        FriendGroups = friendGroups;
        NextUin = nextUin;
    }

    public static FetchFriendsEvent Create(uint? nextUin = null) => new(nextUin);

    public static FetchFriendsEvent Result(int resultCode, List<BotFriend> friends, Dictionary<uint, string> friendGroups, uint? nextUin) =>
        new(resultCode, friends, friendGroups, nextUin);
}