using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Generic;
using MessagePack;

namespace Lagrange.OneBot.Core.Entity.MetaEvent;

internal class HeartBeat : OneBotRequest
{
    private const string EventType = "meta";
    
    private const string EventDetailType = "heartbeat";

    private const string EventSubType = "";
    
    public HeartBeat(long interval) : base(EventType, EventDetailType, EventSubType) => Interval = interval;

    [JsonPropertyName("interval")] [Key("interval")] public long Interval { get; }
}