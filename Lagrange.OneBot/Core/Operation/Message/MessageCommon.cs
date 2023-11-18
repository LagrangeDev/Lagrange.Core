using System.Reflection;
using System.Text.Json;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

public static class MessageCommon
{
    private static readonly Dictionary<string, ISegment> TypeToSegment;
    
    static MessageCommon()
    {
        TypeToSegment = new Dictionary<string, ISegment>();
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<SegmentSubscriberAttribute>();
            if (attribute != null) TypeToSegment[attribute.SendType] = (ISegment)type.CreateInstance(false);
        }
    }
    
    public static MessageBuilder ParseChain(OneBotMessage message)
    {
        MessageBuilder builder;
        if (message.MessageType != "")
        {
            builder = message.MessageType == "private"
                ? MessageBuilder.Friend(message.UserId ?? 0)
                : MessageBuilder.Group(message.GroupId ?? 0);
        }
        else
        {
            builder = message.UserId != null
                ? MessageBuilder.Friend(message.UserId ?? 0)
                : MessageBuilder.Group(message.GroupId ?? 0);
        }
        BuildMessages(builder, message.Messages);

        return builder;
    }
    
    public static MessageBuilder ParseChain(OneBotPrivateMessage message)
    {
        var builder = MessageBuilder.Friend(message.UserId);
        BuildMessages(builder, message.Messages);

        return builder;
    }
    
    public static MessageBuilder ParseChain(OneBotGroupMessage message)
    {
        var builder = MessageBuilder.Group(message.GroupId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    private static void BuildMessages(MessageBuilder builder, List<OneBotSegment> segments)
    {
        foreach (var segment in segments)
        {
            if (TypeToSegment.TryGetValue(segment.Type, out var instance))
            {
                var cast = (ISegment)((JsonElement)segment.Data).Deserialize(instance.GetType())!;
                instance.Build(builder, cast);
            }
        }
    }
}