using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_msg")]
public sealed class SendMessageOperation : IOperation
{
    private readonly Dictionary<string, ISegment> _typeToSegment;
    
    public SendMessageOperation()
    {
        _typeToSegment = new Dictionary<string, ISegment>();
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<SegmentSubscriberAttribute>();
            if (attribute != null) _typeToSegment[attribute.Type] = (ISegment)type.CreateInstance(false);
        }
    }
    
    public OneBotResult HandleOperation(string echo, BotContext context, JsonObject? payload)
    {
        var message = payload.Deserialize<OneBotMessage>();
        if (message != null)
        {
            context.SendMessage(ParseChain(message).Build());
            return new OneBotResult(new OneBotMessageResponse(0), 0, "ok", echo);
        }

        throw new Exception();
    }

    private MessageBuilder ParseChain(OneBotMessage message)
    {
        var builder = message.MessageType == "private"
            ? MessageBuilder.Friend(message.UserId)
            : MessageBuilder.Group(message.GroupId);
        
        foreach (var segment in message.Messages)
        {
            if (_typeToSegment.TryGetValue(segment.Type, out var instance))
            {
                var cast = (ISegment)((JsonElement)segment.Data).Deserialize(instance.GetType())!;
                instance.Build(builder, cast);
            }
        }

        return builder;
    }
}