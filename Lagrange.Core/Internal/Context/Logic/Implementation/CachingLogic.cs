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

    private readonly Dictionary<uint, SysFaceEntry> _cacheFaceEntities;
    private readonly List<uint> _cacheSuperFaceId;

    internal CachingLogic(ContextCollection collection) : base(collection)
    {
        _uinToUid = new Dictionary<uint, string>();
        _cachedGroups = new List<uint>();
        _cachedGroupEntities = new List<BotGroup>();

        _cachedFriends = new List<BotFriend>();
        _cachedGroupMembers = new Dictionary<uint, List<BotGroupMember>>();

        _cacheUsers = new ConcurrentDictionary<uint, BotUserInfo>();

        _cacheFaceEntities = new Dictionary<uint, SysFaceEntry>();
        _cacheSuperFaceId = new List<uint>();
    }

    public override Task Incoming(ProtocolEvent e)
    {
        return e switch
        {
            GroupSysDecreaseEvent groupSysDecreaseEvent when groupSysDecreaseEvent.MemberUid != Collection.Keystore.Uid => CacheUid(groupSysDecreaseEvent.GroupUin, true),
            GroupSysIncreaseEvent groupSysIncreaseEvent => CacheUid(groupSysIncreaseEvent.GroupUin, true),
            GroupSysAdminEvent groupSysAdminEvent => CacheUid(groupSysAdminEvent.GroupUin, true),
            _ => Task.CompletedTask,
        };
    }

    public async Task<List<BotGroup>> GetCachedGroups(bool refreshCache)
    {
        if (_cachedGroupEntities.Count == 0 || refreshCache)
        {
            _cachedGroupEntities.Clear();

            var fetchGroupsEvent = FetchGroupsEvent.Create();
            var events = await Collection.Business.SendEvent(fetchGroupsEvent);
            var groups = ((FetchGroupsEvent)events[0]).Groups;

            _cachedGroupEntities.AddRange(groups);
            return groups;
        }

        return _cachedGroupEntities;
    }

    public async Task<string?> ResolveUid(uint? groupUin, uint friendUin)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUidAndFriendGroups();
        if (groupUin == null) return _uinToUid.GetValueOrDefault(friendUin);

        await CacheUid(groupUin.Value);

        return _uinToUid.GetValueOrDefault(friendUin);
    }

    public async Task<uint?> ResolveUin(uint? groupUin, string friendUid, bool force = false)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUidAndFriendGroups();
        if (groupUin == null) return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;

        await CacheUid(groupUin.Value, force);

        return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;
    }

    public async Task<List<BotGroupMember>> GetCachedMembers(uint groupUin, bool refreshCache)
    {
        if (!_cachedGroupMembers.TryGetValue(groupUin, out var members) || refreshCache)
        {
            await ResolveMembersUid(groupUin);
            return _cachedGroupMembers.TryGetValue(groupUin, out members) ? members : new List<BotGroupMember>();
        }

        return members;
    }

    public async Task<List<BotFriend>> GetCachedFriends(bool refreshCache)
    {
        if (_cachedFriends.Count == 0 || refreshCache) await ResolveFriendsUidAndFriendGroups();
        return _cachedFriends;
    }

    public async Task<BotUserInfo?> GetCachedUsers(uint uin, bool refreshCache)
    {
        if (!_cacheUsers.ContainsKey(uin) || refreshCache) await ResolveUser(uin);
        if (!_cacheUsers.TryGetValue(uin, out BotUserInfo? info)) return null;
        return info;
    }

    private async Task CacheUid(uint groupUin, bool force = false)
    {
        if (!_cachedGroups.Contains(groupUin) || force)
        {
            Collection.Log.LogVerbose(Tag, $"Caching group members: {groupUin}");
            await ResolveMembersUid(groupUin);
            _cachedGroups.Add(groupUin);
        }
    }

    private async Task ResolveFriendsUidAndFriendGroups()
    {
        uint? next = null;
        var friends = new List<BotFriend>();
        var friendGroups = new Dictionary<uint, string>();
        do
        {
            var @event = FetchFriendsEvent.Create(next);
            var results = await Collection.Business.SendEvent(@event);

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

    private async Task ResolveMembersUid(uint groupUin)
    {
        var fetchFriendsEvent = FetchMembersEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(fetchFriendsEvent);

        if (events.Count != 0)
        {
            var @event = (FetchMembersEvent)events[0];
            string? token = @event.Token;

            while (token != null)
            {
                var next = FetchMembersEvent.Create(groupUin, token);
                var results = await Collection.Business.SendEvent(next);
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

    private async Task ResolveUser(uint uin)
    {
        var events = await Collection.Business.SendEvent(FetchUserInfoEvent.Create(uin));

        if (events.Count != 0 && events[0] is FetchUserInfoEvent { } @event)
        {
            _cacheUsers.AddOrUpdate(uin, @event.UserInfo, (_key, _value) => @event.UserInfo);
        }
    }

    private async Task ResolveEmojiCache()
    {
        var fetchSysEmojisEvent = FetchFullSysFacesEvent.Create();
        var events = await Collection.Business.SendEvent(fetchSysEmojisEvent);
        var emojiPacks = ((FetchFullSysFacesEvent)events[0]).FacePacks;

        emojiPacks
            .SelectMany(pack => pack.Emojis)
            .Where(emoji => uint.TryParse(emoji.QSid, out _))
            .ToList()
            .ForEach(emoji => _cacheFaceEntities[uint.Parse(emoji.QSid)] = emoji);

        _cacheSuperFaceId.AddRange(emojiPacks
            .SelectMany(emojiPack => emojiPack.GetUniqueSuperQSids(new[] { (1, 1) })));
    }

    public async Task<bool> GetCachedIsSuperFaceId(uint id)
    {
        if (!_cacheSuperFaceId.Any()) await ResolveEmojiCache();
        return _cacheSuperFaceId.Contains(id);
    }

    public async Task<SysFaceEntry?> GetCachedFaceEntity(uint faceId)
    {
        if (!_cacheFaceEntities.ContainsKey(faceId)) await ResolveEmojiCache();
        return _cacheFaceEntities.GetValueOrDefault(faceId);
    }
}