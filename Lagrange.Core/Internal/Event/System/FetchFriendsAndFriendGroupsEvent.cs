using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchFriendsAndFriendGroupsEvent : ProtocolEvent
{
    public List<BotFriend> Friends { get; } = new();

    public Dictionary<uint, string> FriendGroups { get; } = new();

    public uint? NextUin { get; }

    private FetchFriendsAndFriendGroupsEvent(uint? nextUin) : base(true)
    {
        NextUin = nextUin;
    }

    private FetchFriendsAndFriendGroupsEvent(int resultCode, List<BotFriend> friends, Dictionary<uint, string> friendGroups, uint? nextUin) : base(resultCode)
    {
        Friends = friends;
        FriendGroups = friendGroups;
        NextUin = nextUin;
    }

    public static FetchFriendsAndFriendGroupsEvent Create(uint? nextUin = null) => new(nextUin);

    public static FetchFriendsAndFriendGroupsEvent Result(int resultCode, List<BotFriend> friends, Dictionary<uint, string> friendGroups, uint? nextUin) =>
        new(resultCode, friends, friendGroups, nextUin);
}