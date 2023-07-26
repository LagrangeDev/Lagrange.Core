using Lagrange.Core.Core.Context.Attributes;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Service;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Core.Event;
using Lagrange.Core.Core.Event.EventArg;
using System.Text;

namespace Lagrange.Core.Core.Context.Logic.Implementation;

[EventSubscribe(typeof(PushMessageEvent))]
[EventSubscribe(typeof(SendMessageEvent))]
[BusinessLogic("MessagingLogic", "Manage the receiving and sending of messages")]
internal class MessagingLogic : LogicBase
{
    private const string Tag = nameof(MessagingLogic);

    private readonly Dictionary<uint, string> _uinToUid;
    private readonly List<uint> _cachedGroups;

    internal MessagingLogic(ContextCollection collection) : base(collection)
    {
        _uinToUid = new Dictionary<uint, string>();
        _cachedGroups = new List<uint>();
    }

    public override async Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case PushMessageEvent push:
                if (push.Chain.Count == 0) return;
                await ResolveAdditionalPackets(push.Chain);

                #region Debug Console Output

                var chain = push.Chain;
                var chainBuilder = new StringBuilder();

                chainBuilder.Append("[MessageChain");
                if (chain.GroupUin != null) chainBuilder.Append($"({chain.GroupUin})");
                chainBuilder.Append($"({chain.FriendUin})");
                chainBuilder.Append("] ");
                foreach (var entity in chain)
                {
                    chainBuilder.Append(entity.ToPreviewString());
                    if (chain.Last() != entity) chainBuilder.Append(" | ");
                }
                Collection.Log.LogVerbose(Tag, chainBuilder.ToString());

                #endregion

                EventBase args = push.Chain.GroupUin != null
                    ? new GroupMessageEvent(push.Chain)
                    : new FriendMessageEvent(push.Chain);
                Collection.Invoker.PostEvent(args);
                
                break;
        }
    }

    public override async Task Outgoing(ProtocolEvent e)
    {
        switch (e)
        {
            case SendMessageEvent send: // resolve Uin to Uid
                if (_uinToUid.Count == 0) await ResolveFriendsUid();

                if (send.Chain.GroupUin != null && !_cachedGroups.Contains(send.Chain.GroupUin.Value))
                {
                    await ResolveMembersUid(send.Chain.GroupUin.Value);
                    _cachedGroups.Add(send.Chain.GroupUin.Value);
                }

                ResolveChainUid(send.Chain); 
                break;
        }
    }

    private async Task ResolveAdditionalPackets(MessageChain chain)
    {
        if (chain.HasTypeOf<FileEntity>())
        {
            var file = chain.GetEntity<FileEntity>();
            if (file?.IsGroup != false || file.FileHash == null || file.FileUuid == null) return;
            
            var @event = FileDownloadEvent.Create(file.FileUuid, file.FileHash, chain.Uid, chain.SelfUid);
            var results = await Collection.Business.SendEvent(@event);
            if (results.Count != 0)
            {
                var result = (FileDownloadEvent)results[0];
            }
        }
    }

    private void ResolveChainUid(MessageChain chain)
    {
        foreach (var entity in chain)
        {
            switch (entity)
            {
                case MentionEntity mention:
                    mention.Uid = _uinToUid[mention.Uin];
                    break;
            }
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