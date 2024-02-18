using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Message;
using LiteDB;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lagrange.OneBot.Core.Operation.Message;

public partial class MessageCommon
{
    private readonly ILogger<MessageCommon> _logger;
    
    private readonly Dictionary<string, SegmentBase> _typeToSegment;

    public MessageCommon(LiteDatabase database, ILogger<MessageCommon> logger)
    {
        _logger = logger;
        _typeToSegment = new Dictionary<string, SegmentBase>();

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.GetCustomAttribute<SegmentSubscriberAttribute>() is { } attribute)
            {
                var instance = (SegmentBase)type.CreateInstance(false);
                instance.Database = database;
                _typeToSegment[attribute.SendType] = instance;
            }
        }
    }
    
    public MessageBuilder ParseFakeChain(OneBotFakeNode message)
    {
        var builder = MessageBuilder.Friend(uint.Parse(message.Uin));
        BuildMessages(builder, message.Content);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotMessage message)
    {
        var builder = message.MessageType == "private" || message.GroupId == null
            ? MessageBuilder.Friend(message.UserId ?? 0)
            : MessageBuilder.Group(message.GroupId.Value);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotMessageSimple message)
    {
        var builder = message.MessageType == "private" || message.GroupId == null
            ? MessageBuilder.Friend(message.UserId ?? 0)
            : MessageBuilder.Group(message.GroupId.Value);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotMessageText message)
    {
        var builder = message.MessageType == "private" || message.GroupId == null
            ? MessageBuilder.Friend(message.UserId ?? 0)
            : MessageBuilder.Group(message.GroupId.Value);
        
        if (message.AutoEscape == true)
        {
            builder.Text(message.Messages);
        }
        else
        {
            BuildMessages(builder, message.Messages);
        }

        return builder;
    }

    public MessageBuilder ParseChain(OneBotPrivateMessage message)
    {
        var builder = MessageBuilder.Friend(message.UserId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotPrivateMessageSimple message)
    {
        var builder = MessageBuilder.Friend(message.UserId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotPrivateMessageText message)
    {
        var builder = MessageBuilder.Friend(message.UserId);
        if (message.AutoEscape == true)
        {
            builder.Text(message.Messages);
        }
        else
        {
            BuildMessages(builder, message.Messages);
        }
        return builder;
    }


    public MessageBuilder ParseChain(OneBotGroupMessage message)
    {
        var builder = MessageBuilder.Group(message.GroupId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotGroupMessageSimple message)
    {
        var builder = MessageBuilder.Group(message.GroupId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public MessageBuilder ParseChain(OneBotGroupMessageText message)
    {
        var builder = MessageBuilder.Group(message.GroupId);
        if (message.AutoEscape == true)
        {
            builder.Text(message.Messages);
        }
        else
        {
            BuildMessages(builder, message.Messages);
        }
        return builder;
    }

    [GeneratedRegex(@"\[CQ:([^,\]]+)(?:,([^,\]]+))*\]")]
    private static partial Regex CQCodeRegex();

    private static string UnescapeCQ(string str) => str.Replace("&#91;", "[")
        .Replace("&#93;", "]")
        .Replace("&#44;", ",")
        .Replace("&amp;", "&");

    private static string UnescapeText(string str) => str.Replace("&#91;", "[")
        .Replace("&#93;", "]")
        .Replace("&amp;", "&");

    private void BuildMessages(MessageBuilder builder, string message)
    {
        var matches = CQCodeRegex().Matches(message);
        int textStart = 0;
        foreach (var match in matches.Cast<Match>())
        {
            if (match.Index > textStart) builder.Text(UnescapeText(message[textStart..match.Index]));
            textStart = match.Index + match.Length;

            string type = match.Groups[1].Value;
            if (_typeToSegment.TryGetValue(type, out var instance))
            {
                var data = new Dictionary<string, string>();
                foreach (var capture in match.Groups[2].Captures.Cast<Capture>())
                {
                    var pair = capture.Value.Split('=', 2);
                    if (pair.Length == 2) data[pair[0]] = UnescapeCQ(pair[1]);
                }
                
                if (JsonSerializer.SerializeToElement(data).Deserialize(instance.GetType()) is SegmentBase cast) instance.Build(builder, cast);
                else Log.LogCQFailed(_logger, type, string.Empty);
            }
        }

        if (textStart < message.Length) builder.Text(UnescapeText(message[textStart..]));
    }

    private void BuildMessages(MessageBuilder builder, List<OneBotSegment> segments)
    {
        foreach (var segment in segments)
        {
            if (_typeToSegment.TryGetValue(segment.Type, out var instance))
            {
                if (((JsonElement)segment.Data).Deserialize(instance.GetType()) is SegmentBase cast) instance.Build(builder, cast);
                else Log.LogCQFailed(_logger, segment.Type, string.Empty);
            }
        }
    }

    private void BuildMessages(MessageBuilder builder, OneBotSegment segment)
    {
        if (_typeToSegment.TryGetValue(segment.Type, out var instance))
        {
            if (((JsonElement)segment.Data).Deserialize(instance.GetType()) is SegmentBase cast) instance.Build(builder, cast);
            else Log.LogCQFailed(_logger, segment.Type, string.Empty);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "Segment {type} Deserialization failed for code {code}")]
        public static partial void LogCQFailed(ILogger logger, string type, string code);
    }
}
