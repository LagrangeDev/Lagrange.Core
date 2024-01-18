using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

public static partial class MessageCommon
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
        var builder = message.MessageType == "private"
            ? MessageBuilder.Friend(message.UserId ?? 0)
            : MessageBuilder.Group(message.GroupId ?? 0);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotMessageSimple message)
    {
        var builder = message.MessageType == "private"
            ? MessageBuilder.Friend(message.UserId ?? 0)
            : MessageBuilder.Group(message.GroupId ?? 0);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotMessageText message)
    {
        var builder = message.MessageType == "private"
            ? MessageBuilder.Friend(message.UserId ?? 0)
            : MessageBuilder.Group(message.GroupId ?? 0);
        builder.Text(message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotPrivateMessage message)
    {
        var builder = MessageBuilder.Friend(message.UserId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotPrivateMessageSimple message)
    {
        var builder = MessageBuilder.Friend(message.UserId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotPrivateMessageText message)
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


    public static MessageBuilder ParseChain(OneBotGroupMessage message)
    {
        var builder = MessageBuilder.Group(message.GroupId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotGroupMessageSimple message)
    {
        var builder = MessageBuilder.Group(message.GroupId);
        BuildMessages(builder, message.Messages);

        return builder;
    }

    public static MessageBuilder ParseChain(OneBotGroupMessageText message)
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

    private static string UnescapeCQ(string str)
    {
        return str.Replace("&#91;", "[")
                  .Replace("&#93;", "]")
                  .Replace("&#44;", ",")
                  .Replace("&amp;", "&");
    }

    private static string UnescapeText(string str)
    {
        return str.Replace("&#91;", "[")
                  .Replace("&#93;", "]")
                  .Replace("&amp;", "&");
    }

    private static void BuildMessages(MessageBuilder builder, string message)
    {
        var matches = CQCodeRegex().Matches(message);
        int textStart = 0;
        foreach (var match in matches.Cast<Match>())
        {
            if (match.Index > textStart) builder.Text(UnescapeText(message[textStart..match.Index]));
            textStart = match.Index + match.Length;

            string type = match.Groups[1].Value;
            if (TypeToSegment.TryGetValue(type, out var instance))
            {
                var data = new Dictionary<string, string>();
                foreach (var capture in match.Groups[2].Captures.Cast<Capture>())
                {
                    var pair = capture.Value.Split('=', 2);
                    if (pair.Length == 2) data[pair[0]] = UnescapeCQ(pair[1]);
                }
                var cast = (ISegment)JsonSerializer.SerializeToElement(data).Deserialize(instance.GetType())!;
                instance.Build(builder, cast);
            }
        }

        if (textStart < message.Length) builder.Text(UnescapeText(message[textStart..]));
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

    private static void BuildMessages(MessageBuilder builder, OneBotSegment segment)
    {
        if (TypeToSegment.TryGetValue(segment.Type, out var instance))
        {
            var cast = (ISegment)((JsonElement)segment.Data).Deserialize(instance.GetType())!;
            instance.Build(builder, cast);
        }
    }
}
