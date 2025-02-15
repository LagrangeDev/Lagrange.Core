using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
[JsonConverter(typeof(OneBotMessageConverter<OneBotMessageBase, OneBotMessage, OneBotMessageSimple, OneBotMessageText>))]
public class OneBotMessageBase
{
    [JsonPropertyName("message_type")] public string MessageType { get; set; } = "";
    
    [JsonPropertyName("user_id")] public uint? UserId { get; set; }
    
    [JsonPropertyName("group_id")] public uint? GroupId { get; set; }
    
    [JsonPropertyName("auto_escape")] public bool? AutoEscape { get; set; }

    [JsonPropertyName("message_style")] public OnebotMessageStyle? MessageStyle { get; set; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OneBotMessage : OneBotMessageBase
{
    [JsonPropertyName("message")] public List<OneBotSegment> Messages { get; set; } = new();
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OneBotMessageSimple : OneBotMessageBase
{
    [JsonPropertyName("message")] public OneBotSegment Messages { get; set; } = new();
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OneBotMessageText : OneBotMessageBase
{
    [JsonPropertyName("message")] public string Messages { get; set; } = "";
}

public class OneBotMessageConverter<T, TList, TSimple, TText> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Utf8JsonReader readerClone = reader;
        while (readerClone.Read())
        {
            if (readerClone.TokenType != JsonTokenType.PropertyName) continue;
            if (readerClone.GetString() != "message") continue;
            readerClone.Read();
            switch (readerClone.TokenType)
            {
                case JsonTokenType.StartArray:
                    // List<Message>
                    return (T?)JsonSerializer.Deserialize(ref reader, typeof(TList), options);
                case JsonTokenType.StartObject:
                    // Message
                    return (T?)JsonSerializer.Deserialize(ref reader, typeof(TSimple), options);
                case JsonTokenType.String:
                    // String
                    return (T?)JsonSerializer.Deserialize(ref reader, typeof(TText), options);
                default:
                    throw new Exception();
            }
        }
        
        throw new Exception("json error");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // Null
        
    }
}