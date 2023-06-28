using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Generic;
using MessagePack;

namespace Lagrange.OneBot.Core.Entity.MetaEvent;

internal class Connect : OneBotRequest
{
    private const string EventType = "meta";
    
    private const string EventDetailType = "connect";

    private const string EventSubType = "";
    
    public Connect() : base(EventType, EventDetailType, EventSubType) { }
    
    [JsonPropertyName("version")] [Key("version")] public OneBotVersion Version { get; set; } = new();
}