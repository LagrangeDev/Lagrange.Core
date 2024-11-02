using System.Text.Json.Serialization;
using static Lagrange.Core.Event.EventArg.BotOnlineEvent;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotBotOnline(uint selfId, OnlineReason reason) : OneBotNotify(selfId, "bot_online")
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("reason")]
    public OnlineReason Reason { get; } = reason;
}