using System.Collections.Concurrent;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;

namespace Lagrange.Core.Internal.Context;

internal class CacheContext(BotContext context)
{
    private List<BotFriend>? _friends;

    private List<BotGroup>? _groups;

    private readonly ConcurrentDictionary<long, string> _uinToUid = new();
    private readonly ConcurrentDictionary<string, long> _uidToUin = new();

    private readonly ConcurrentDictionary<long, List<BotGroupMember>> _members = new();

    private readonly Dictionary<int, BotFriendCategory> _categories = [];

    private readonly Dictionary<string, BotStranger> _strangersWithUid = [];
    private readonly Dictionary<long, BotStranger> _strangersWithUin = [];
    private readonly SemaphoreSlim _strangersLock = new(1);

    public async Task<List<BotFriend>> GetFriendList(bool refresh = false)
    {
        if (refresh || _friends == null) Interlocked.Exchange(ref _friends, await FetchFriends());

        return _friends;
    }

    public async Task<List<BotGroup>> GetGroupList(bool refresh = false)
    {
        if (refresh) Interlocked.Exchange(ref _groups, await FetchGroups());
        Interlocked.CompareExchange(ref _groups, await FetchGroups(), null);

        return _groups;
    }

    public async Task<List<BotGroupMember>> GetMemberList(long groupUin, bool refresh = false)
    {
        if (refresh || !_members.TryGetValue(groupUin, out var members))
        {
            members = _members[groupUin] = await FetchGroupMembers(groupUin);
        }

        return members;
    }

    public async Task<List<BotFriendCategory>> GetCategories(bool refresh = false)
    {
        if (refresh || _categories.Count == 0) Interlocked.Exchange(ref _friends, await FetchFriends());

        return _categories.Values.ToList();
    }

    public async Task<BotFriend?> ResolveFriend(long uin)
    {
        if (_friends == null) Interlocked.Exchange(ref _friends, await FetchFriends());
        var friend = _friends?.FirstOrDefault(f => f.Uin == uin);

        if (friend == null)
        {
            Interlocked.Exchange(ref _friends, await FetchFriends());
            friend = _friends?.FirstOrDefault(f => f.Uin == uin);
        }

        return friend;
    }

    public async Task<(BotGroup, BotGroupMember)?> ResolveMember(long groupUin, long memberUin)
    {
        var group = await ResolveGroup(groupUin);
        if (group == null) return null;

        if (!_members.TryGetValue(groupUin, out var members))
        {
            members = _members[groupUin] = await FetchGroupMembers(groupUin);
        }
        var member = members.FirstOrDefault(m => m.Uin == memberUin);
        return member == null ? null : (group, member);
    }

    public async Task<BotGroup?> ResolveGroup(long groupUin)
    {
        if (_groups == null) Interlocked.Exchange(ref _groups, await FetchGroups());
        var group = _groups?.FirstOrDefault(f => f.GroupUin == groupUin);

        if (group == null)
        {
            Interlocked.Exchange(ref _groups, await FetchGroups());
            group = _groups?.FirstOrDefault(f => f.GroupUin == groupUin);
        }

        return group;
    }

    public async Task<BotStranger> ResolveStranger(long uin)
    {
        await _strangersLock.WaitAsync();
        try
        {
            if (_strangersWithUin.TryGetValue(uin, out BotStranger? stranger)) return stranger;

            stranger = await FetchStranger(uin);
            _strangersWithUin.Add(uin, stranger);

            return stranger;
        }
        finally { _strangersLock.Release(); }
    }

    public async Task<BotStranger> ResolveStranger(string uid)
    {
        await _strangersLock.WaitAsync();
        try
        {
            if (_strangersWithUid.TryGetValue(uid, out BotStranger? stranger)) return stranger;

            stranger = await FetchStranger(uid);
            _strangersWithUin.TryAdd(stranger.Uin, stranger);
            _strangersWithUid.Add(uid, stranger);

            return stranger;
        }
        finally { _strangersLock.Release(); }
    }

    public string? ResolveCachedUid(long uin) => _uinToUid.GetValueOrDefault(uin);

    public long ResolveUin(string uid)
    {
        if (_uidToUin.TryGetValue(uid, out long value)) return value;

        long uin = _uinToUid.FirstOrDefault(kvp => kvp.Value == uid).Key;
        if (uin != 0) return uin;

        return ResolveStranger(uid).GetAwaiter().GetResult().Uin;
    }

    /// <summary>
    /// Fetches the friends list from the server.
    /// </summary>
    private async Task<List<BotFriend>> FetchFriends()
    {
        var friends = new List<BotFriend>();

        byte[]? cookie = null;
        do
        {
            var result = await context.EventContext.SendEvent<FetchFriendsEventResp>(new FetchFriendsEventReq(cookie));
            cookie = result.Cookie;

            friends.AddRange(result.Friends);
            foreach (var category in result.Category) _categories[category.Id] = category;
            foreach (var friend in friends) _uinToUid[friend.Uin] = friend.Uid;
        } while (cookie != null);

        return friends;
    }

    private async Task<List<BotGroup>> FetchGroups()
    {
        var result = await context.EventContext.SendEvent<FetchGroupsEventResp>(new FetchGroupsEventReq());
        return result.Groups;
    }

    private async Task<List<BotGroupMember>> FetchGroupMembers(long groupUin)
    {
        var members = new List<BotGroupMember>();

        byte[]? cookie = null;
        do
        {
            var result = await context.EventContext.SendEvent<FetchGroupMembersEventResp>(new FetchGroupMembersEventReq(groupUin, cookie));
            cookie = result.Cookie;

            members.AddRange(result.GroupMembers);
            foreach (var member in result.GroupMembers) _uinToUid[member.Uin] = member.Uid;
        } while (cookie != null);

        return members;
    }

    private async Task<BotStranger> FetchStranger(long uin)
    {
        var result = await context.EventContext.SendEvent<FetchStrangerEventResp>(new FetchStrangerByUinEventReq(uin));
        return result.Stranger;
    }

    private async Task<BotStranger> FetchStranger(string uid)
    {
        var result = await context.EventContext.SendEvent<FetchStrangerEventResp>(new FetchStrangerByUidEventReq(uid));
        return result.Stranger;
    }
}