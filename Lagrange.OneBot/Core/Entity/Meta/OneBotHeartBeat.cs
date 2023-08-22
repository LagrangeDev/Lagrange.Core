using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public class OneBotHeartBeat : OneBotMeta
{
    [JsonPropertyName("interval")] public int Interval { get; set; }
    
    [JsonPropertyName("status")] public object Status { get; set; }
    
    public OneBotHeartBeat(uint selfId, int interval, object status) : base(selfId, "heartbeat")
    {
        Interval = interval;
        Status = status;
    }
}