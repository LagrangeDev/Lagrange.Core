using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
[JsonConverter(typeof(OneBotMessageConverter<OneBotPrivateMessageBase, OneBotPrivateMessage, OneBotPrivateMessageSimple, OneBotPrivateMessageText>))]
public class OneBotPrivateMessageBase
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("auto_escape")] public bool? AutoEscape { get; set; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OneBotPrivateMessage : OneBotPrivateMessageBase
{
    [JsonPropertyName("message")] public List<OneBotSegment> Messages { get; set; } = new();
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OneBotPrivateMessageSimple : OneBotPrivateMessageBase
{
    [JsonPropertyName("message")] public OneBotSegment Messages { get; set; } = new();
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OneBotPrivateMessageText : OneBotPrivateMessageBase
{
    [JsonPropertyName("message")] public string Messages { get; set; } = "";
}