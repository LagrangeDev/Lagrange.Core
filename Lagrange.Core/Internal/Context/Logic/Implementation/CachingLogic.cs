using System.Collections.Concurrent;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Service;

namespace Lagrange.Core.Internal.Context.Logic.Implementation;

[EventSubscribe(typeof(GroupSysDecreaseEvent))]
[EventSubscribe(typeof(GroupSysIncreaseEvent))]
[EventSubscribe(typeof(GroupSysAdminEvent))]
[BusinessLogic("CachingLogic", "Cache Uin to Uid")]
internal class CachingLogic : LogicBase
{
    private const string Tag = nameof(CachingLogic);

    private readonly Dictionary<uint, string> _uinToUid;
    private readonly List<uint> _cachedGroups;
    private readonly List<BotGroup> _cachedGroupEntities;

    private readonly List<BotFriend> _cachedFriends;
    private readonly Dictionary<uint, List<BotGroupMember>> _cachedGroupMembers;

    private readonly ConcurrentDictionary<uint, BotUserInfo> _cacheUsers;

    internal CachingLogic(ContextCollection collection) : base(collection)
    {
        _uinToUid = new Dictionary<uint, string>();
        _cachedGroups = new List<uint>();
        _cachedGroupEntities = new List<BotGroup>();

        _cachedFriends = new List<BotFriend>();
        _cachedGroupMembers = new Dictionary<uint, List<BotGroupMember>>();

        _cacheUsers = new ConcurrentDictionary<uint, BotUserInfo>();
    }

    public override Task Incoming(ProtocolEvent e, CancellationToken ct)
    {
        return e switch
        {
            GroupSysDecreaseEvent groupSysDecreaseEvent when groupSysDecreaseEvent.MemberUid != Collection.Keystore.Uid => CacheUid(groupSysDecreaseEvent.GroupUin, ct, force: true),
            GroupSysIncreaseEvent groupSysIncreaseEvent => CacheUid(groupSysIncreaseEvent.GroupUin, ct, force: true),
            GroupSysAdminEvent groupSysAdminEvent => CacheUid(groupSysAdminEvent.GroupUin, ct, force: true),
            _ => Task.CompletedTask,
        };
    }

    public async Task<List<BotGroup>> GetCachedGroups(bool refreshCache, CancellationToken ct)
    {
        if (_cachedGroupEntities.Count == 0 || refreshCache)
        {
            _cachedGroupEntities.Clear();

            var fetchGroupsEvent = FetchGroupsEvent.Create();
            var events = await Collection.Business.SendEvent(fetchGroupsEvent, ct);
            var groups = ((FetchGroupsEvent)events[0]).Groups;

            _cachedGroupEntities.AddRange(groups);
            return groups;
        }

        return _cachedGroupEntities;
    }

    public async Task<string?> ResolveUid(uint? groupUin, uint friendUin, CancellationToken ct)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUidAndFriendGroups(ct);
        if (groupUin == null) return _uinToUid.GetValueOrDefault(friendUin);

        await CacheUid(groupUin.Value, ct);

        return _uinToUid.GetValueOrDefault(friendUin);
    }

    public async Task<uint?> ResolveUin(uint? groupUin, string friendUid, CancellationToken ct, bool force = false)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUidAndFriendGroups(ct);
        if (groupUin == null) return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;

        await CacheUid(groupUin.Value, ct, force: force);

        return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;
    }

    public async Task<List<BotGroupMember>> GetCachedMembers(uint groupUin, bool refreshCache, CancellationToken ct)
    {
        if (!_cachedGroupMembers.TryGetValue(groupUin, out var members) || refreshCache)
        {
            await ResolveMembersUid(groupUin, ct);
            return _cachedGroupMembers.TryGetValue(groupUin, out members) ? members : new List<BotGroupMember>();
        }
        return members;
    }

    public async Task<List<BotFriend>> GetCachedFriends(bool refreshCache, CancellationToken ct)
    {
        if (_cachedFriends.Count == 0 || refreshCache) await ResolveFriendsUidAndFriendGroups(ct);
        return _cachedFriends;
    }

    public async Task<BotUserInfo?> GetCachedUsers(uint uin, bool refreshCache, CancellationToken ct)
    {
        if (!_cacheUsers.ContainsKey(uin) || refreshCache) await ResolveUser(uin, ct);
        if (!_cacheUsers.TryGetValue(uin, out BotUserInfo? info)) return null;
        return info;
    }

    private async Task CacheUid(uint groupUin, CancellationToken ct, bool force = false)
    {
        if (!_cachedGroups.Contains(groupUin) || force)
        {
            Collection.Log.LogVerbose(Tag, $"Caching group members: {groupUin}");
            await ResolveMembersUid(groupUin, ct);
            _cachedGroups.Add(groupUin);
        }
    }

    private async Task ResolveFriendsUidAndFriendGroups(CancellationToken ct)
    {
        uint? next = null;
        var friends = new List<BotFriend>();
        var friendGroups = new Dictionary<uint, string>();
        do
        {
            var @event = FetchFriendsEvent.Create(next);
            var results = await Collection.Business.SendEvent(@event, ct);

            if (results.Count == 0)
            {
                Collection.Log.LogWarning(Tag, "Failed to resolve friends uid and cache.");
                return;
            }

            var result = (FetchFriendsEvent)results[0];

            foreach ((uint id, string name) in result.FriendGroups) friendGroups[id] = name;

            foreach (var friend in result.Friends)
            {
                friend.Group = new(friend.Group.GroupId, friendGroups[friend.Group.GroupId]);
            }
            friends.AddRange(result.Friends);

            next = result.NextUin;
        } while (next.HasValue);

        foreach (var friend in friends) _uinToUid.TryAdd(friend.Uin, friend.Uid);

        _cachedFriends.Clear();
        _cachedFriends.AddRange(friends);
    }

    private async Task ResolveMembersUid(uint groupUin, CancellationToken ct)
    {
        var fetchFriendsEvent = FetchMembersEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(fetchFriendsEvent, ct);

        if (events.Count != 0)
        {
            var @event = (FetchMembersEvent)events[0];
            string? token = @event.Token;

            while (token != null)
            {
                var next = FetchMembersEvent.Create(groupUin, token);
                var results = await Collection.Business.SendEvent(next, ct);
                @event.Members.AddRange(((FetchMembersEvent)results[0]).Members);
                token = ((FetchMembersEvent)results[0]).Token;
            }

            foreach (var member in @event.Members) _uinToUid.TryAdd(member.Uin, member.Uid);
            _cachedGroupMembers[groupUin] = @event.Members;
        }
        else
        {
            _cachedGroupMembers[groupUin] = new List<BotGroupMember>();
            Collection.Log.LogWarning(Tag, $"Failed to resolve group {groupUin} members uid and cache.");
        }
    }

    private async Task ResolveUser(uint uin, CancellationToken ct)
    {
        var events = await Collection.Business.SendEvent(FetchUserInfoEvent.Create(uin), ct);

        if (events.Count != 0 && events[0] is FetchUserInfoEvent { } @event)
        {
            _cacheUsers.AddOrUpdate(uin, @event.UserInfo, (_key, _value) => @event.UserInfo);
        }
    }
}