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

    internal MessagingLogic(ContextCollection collection) : base(collection)
    {
    }

    public override async Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case PushMessageEvent push:
                if (push.Chain.Count == 0) return;
                await ResolveAdditionalPackets(push.Chain);


                var chain = push.Chain;
                Collection.Log.LogVerbose(Tag, chain.ToPreviewString());

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
                await ResolveChainUid(send.Chain); 
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
    private async Task ResolveChainUid(MessageChain chain)
    {
        foreach (var entity in chain)
        {
            switch (entity)
            {
                case MentionEntity mention:
                    mention.Uid = await Collection.Business.CachingLogic.ResolveUid(chain.GroupUin, mention.Uin) ?? throw new Exception($"Failed to resolve Uid for Uin {mention.Uin}");
                    break;
            }
        }
    }
}