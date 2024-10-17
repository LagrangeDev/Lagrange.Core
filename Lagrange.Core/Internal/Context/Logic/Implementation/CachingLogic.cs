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

    public override Task Incoming(ProtocolEvent e, CancellationToken cancellationToken)
    {
        return e switch
        {
            GroupSysDecreaseEvent groupSysDecreaseEvent when groupSysDecreaseEvent.MemberUid != Collection.Keystore.Uid => CacheUid(groupSysDecreaseEvent.GroupUin, force: true, cancellationToken: cancellationToken),
            GroupSysIncreaseEvent groupSysIncreaseEvent => CacheUid(groupSysIncreaseEvent.GroupUin, force: true, cancellationToken: cancellationToken),
            GroupSysAdminEvent groupSysAdminEvent => CacheUid(groupSysAdminEvent.GroupUin, force: true, cancellationToken: cancellationToken),
            _ => Task.CompletedTask,
        };
    }

    public async Task<List<BotGroup>> GetCachedGroups(bool refreshCache, CancellationToken cancellationToken)
    {
        if (_cachedGroupEntities.Count == 0 || refreshCache)
        {
            _cachedGroupEntities.Clear();

            var fetchGroupsEvent = FetchGroupsEvent.Create();
            var events = await Collection.Business.SendEvent(fetchGroupsEvent, cancellationToken);
            var groups = ((FetchGroupsEvent)events[0]).Groups;

            _cachedGroupEntities.AddRange(groups);
            return groups;
        }

        return _cachedGroupEntities;
    }

    public async Task<string?> ResolveUid(uint? groupUin, uint friendUin, CancellationToken cancellationToken)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUidAndFriendGroups(cancellationToken);
        if (groupUin == null) return _uinToUid.GetValueOrDefault(friendUin);

        await CacheUid(groupUin.Value, false, cancellationToken);

        return _uinToUid.GetValueOrDefault(friendUin);
    }

    public async Task<uint?> ResolveUin(uint? groupUin, string friendUid, bool force, CancellationToken cancellationToken)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUidAndFriendGroups(cancellationToken);
        if (groupUin == null) return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;

        await CacheUid(groupUin.Value, force: force, cancellationToken: cancellationToken);

        return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;
    }

    public async Task<List<BotGroupMember>> GetCachedMembers(uint groupUin, bool refreshCache, CancellationToken cancellationToken)
    {
        if (!_cachedGroupMembers.TryGetValue(groupUin, out var members) || refreshCache)
        {
            await ResolveMembersUid(groupUin, cancellationToken);
            return _cachedGroupMembers.TryGetValue(groupUin, out members) ? members : new List<BotGroupMember>();
        }
        return members;
    }

    public async Task<List<BotFriend>> GetCachedFriends(bool refreshCache, CancellationToken cancellationToken)
    {
        if (_cachedFriends.Count == 0 || refreshCache) await ResolveFriendsUidAndFriendGroups(cancellationToken);
        return _cachedFriends;
    }

    public async Task<BotUserInfo?> GetCachedUsers(uint uin, bool refreshCache, CancellationToken cancellationToken)
    {
        if (!_cacheUsers.ContainsKey(uin) || refreshCache) await ResolveUser(uin, cancellationToken);
        if (!_cacheUsers.TryGetValue(uin, out BotUserInfo? info)) return null;
        return info;
    }

    private async Task CacheUid(uint groupUin, bool force, CancellationToken cancellationToken)
    {
        if (!_cachedGroups.Contains(groupUin) || force)
        {
            Collection.Log.LogVerbose(Tag, $"Caching group members: {groupUin}");
            await ResolveMembersUid(groupUin, cancellationToken);
            _cachedGroups.Add(groupUin);
        }
    }

    private async Task ResolveFriendsUidAndFriendGroups(CancellationToken cancellationToken)
    {
        uint? next = null;
        var friends = new List<BotFriend>();
        var friendGroups = new Dictionary<uint, string>();
        do
        {
            var @event = FetchFriendsEvent.Create(next);
            var results = await Collection.Business.SendEvent(@event, cancellationToken);

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

    private async Task ResolveMembersUid(uint groupUin, CancellationToken cancellationToken)
    {
        var fetchFriendsEvent = FetchMembersEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(fetchFriendsEvent, cancellationToken);

        if (events.Count != 0)
        {
            var @event = (FetchMembersEvent)events[0];
            string? token = @event.Token;

            while (token != null)
            {
                var next = FetchMembersEvent.Create(groupUin, token);
                var results = await Collection.Business.SendEvent(next, cancellationToken);
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

    private async Task ResolveUser(uint uin, CancellationToken cancellationToken)
    {
        var events = await Collection.Business.SendEvent(FetchUserInfoEvent.Create(uin), cancellationToken);

        if (events.Count != 0 && events[0] is FetchUserInfoEvent { } @event)
        {
            _cacheUsers.AddOrUpdate(uin, @event.UserInfo, (_key, _value) => @event.UserInfo);
        }
    }
}
