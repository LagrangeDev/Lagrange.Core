using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotBotOffline(uint selfId, string tag, string message) : OneBotNotify(selfId, "bot_offline")
{
    [JsonPropertyName("tag")] public string Tag { get; } = tag;

    [JsonPropertyName("message")] public string Message { get; } = message;
}