using Lagrange.Core.Core.Context.Attributes;

namespace Lagrange.Core.Core.Context.Logic.Implementation;

[BusinessLogic("CachingLogic", "Cache Uin to Uid")]
internal class CachingLogic : LogicBase
{
    private const string Tag = nameof(CachingLogic);
    
    private readonly Dictionary<uint, string> _uinToUid;
    private readonly List<uint> _cachedGroups;
    
    internal CachingLogic(ContextCollection collection) : base(collection)
    {
        _uinToUid = new Dictionary<uint, string>();
        _cachedGroups = new List<uint>();
    }

    public async Task<string?> ResolveUid(uint? groupUin, uint friendUin)
    {
        if (_uinToUid.Count == 0) await ResolveFriendsUid();
        if (groupUin == null) return _uinToUid.TryGetValue(friendUin, out var friendUid) ? friendUid : null;

        uint groupUinValue = groupUin.Value;
        if (!_cachedGroups.Contains(groupUinValue))
        {
            Collection.Log.LogVerbose(Tag, $"Caching group members: {groupUinValue}");
            await ResolveMembersUid(groupUinValue);
            _cachedGroups.Add(groupUinValue);
        }
        return _uinToUid.TryGetValue(friendUin, out var uid) ? uid : null;
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