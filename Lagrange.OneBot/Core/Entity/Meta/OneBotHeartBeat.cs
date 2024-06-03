using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public class OneBotHeartBeat(uint selfId, int interval, object status) : OneBotMeta(selfId, "heartbeat")
{
    [JsonPropertyName("interval")] public int Interval { get; set; } = interval;

    [JsonPropertyName("status")] public object Status { get; set; } = status;
}