using Lagrange.Core.Common.Entity;
using Lagrange.Core.Core.Context.Attributes;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Service;

namespace Lagrange.Core.Core.Context.Logic.Implementation;

[EventSubscribe(typeof(InfoPushGroupEvent))]
[BusinessLogic("CachingLogic", "Cache Uin to Uid")]
internal class CachingLogic : LogicBase
{
    private const string Tag = nameof(CachingLogic);
    
    private readonly Dictionary<uint, string> _uinToUid;
    private readonly List<uint> _cachedGroups;
    private readonly List<BotGroup> _cachedGroupEntities;

    private TaskCompletionSource<List<BotGroup>>? _initCompletionSource;
    
    internal CachingLogic(ContextCollection collection) : base(collection)
    {
        _uinToUid = new Dictionary<uint, string>();
        _cachedGroups = new List<uint>();
        _cachedGroupEntities = new List<BotGroup>();
    }

    public override Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case InfoPushGroupEvent infoPushGroupEvent:
                _cachedGroupEntities.Clear();
                _cachedGroupEntities.AddRange(infoPushGroupEvent.Groups);
                
                if (_initCompletionSource != null)
                {
                    _initCompletionSource.SetResult(_cachedGroupEntities);
                    _initCompletionSource = null;
                }
                
                Collection.Log.LogVerbose(Tag, $"Caching group entities: {infoPushGroupEvent.Groups.Count}");
                break;
        }
        
        return Task.CompletedTask;
    }
    
    public Task<List<BotGroup>> GetCachedGroups()
    {
        if (_cachedGroupEntities.Count == 0)
        {
            _initCompletionSource = new TaskCompletionSource<List<BotGroup>>();
            return _initCompletionSource.Task;
        }
        return Task.FromResult(_cachedGroupEntities);
    }

    public async Task<string?> ResolveUid(uint? groupUin, uint friendUin)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUid();
        if (groupUin == null) return _uinToUid.TryGetValue(friendUin, out var friendUid) ? friendUid : null;
        
        await CacheUid(groupUin.Value);

        return _uinToUid.TryGetValue(friendUin, out var uid) ? uid : null;
    }
    
    public async Task<uint?> ResolveUin(uint? groupUin, string friendUid)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUid();
        if (groupUin == null) return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;
        
        await CacheUid(groupUin.Value);

        return _uinToUid.FirstOrDefault(x => x.Value == friendUid).Key;
    }

    public async Task CacheUid(uint groupUin)
    {
        if (!_cachedGroups.Contains(groupUin))
        {
            Collection.Log.LogVerbose(Tag, $"Caching group members: {groupUin}");
            await ResolveMembersUid(groupUin);
            _cachedGroups.Add(groupUin);
        }
    }
    
    private async Task ResolveFriendsUid()
    {
        var friends = await Collection.Business.OperationLogic.FetchFriends();
        
        foreach (var friend in friends) _uinToUid.Add(friend.Uin, friend.Uid);
    }

    private async Task ResolveMembersUid(uint groupUin)
    {
        var members = await Collection.Business.OperationLogic.FetchMembers(groupUin);
        
        foreach (var member in members) _uinToUid.TryAdd(member.Uin, member.Uid);
    }
}