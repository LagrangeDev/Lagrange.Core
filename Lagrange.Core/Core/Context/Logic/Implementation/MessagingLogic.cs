using Lagrange.Core.Core.Context.Attributes;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Service;

namespace Lagrange.Core.Core.Context.Logic.Implementation;

[EventSubscribe(typeof(PushMessageEvent))]
[EventSubscribe(typeof(SendMessageEvent))]
[BusinessLogic("MessagingLogic", "Manage the receiving and sending of messages")]
internal class MessagingLogic : LogicBase
{
    private const string Tag = nameof(MessagingLogic);
    
    internal MessagingLogic(ContextCollection collection) : base(collection) { }

    public override async Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case PushMessageEvent push:
                Collection.Log.LogInfo(Tag, "Message Received, Detail to be implemented");
                break;
            case SendMessageEvent send:
                break;
        }
    }
}