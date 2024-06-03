using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
[JsonConverter(typeof(OneBotMessageConverter<OneBotGroupMessageBase, OneBotGroupMessage, OneBotGroupMessageSimple, OneBotGroupMessageText>))]
public class OneBotGroupMessageBase
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("auto_escape")] public bool? AutoEscape { get; set; }
}

[Serializable]
public class OneBotGroupMessage : OneBotGroupMessageBase
{
    [JsonPropertyName("message")] public List<OneBotSegment> Messages { get; set; } = new();
}

[Serializable]
public class OneBotGroupMessageSimple : OneBotGroupMessageBase
{
    [JsonPropertyName("message")] public OneBotSegment Messages { get; set; } = new();
}

[Serializable]
public class OneBotGroupMessageText : OneBotGroupMessageBase
{
    [JsonPropertyName("message")] public string Messages { get; set; } = "";
}