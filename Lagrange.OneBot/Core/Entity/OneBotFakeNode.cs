using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
[JsonConverter(typeof(OneBotFakeNodeConverter<OneBotFakeNodeBase, OneBotFakeNode, OneBotFakeNodeSimple, OneBotFakeNodeText>))]
public class OneBotFakeNodeBase
{
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("uin")] public string Uin { get; set; } = string.Empty;
}

[Serializable]
public class OneBotFakeNode : OneBotFakeNodeBase
{
    [JsonPropertyName("content")] public List<OneBotSegment> Content { get; set; } = [];
}

[Serializable]
public class OneBotFakeNodeSimple : OneBotFakeNodeBase
{
    [JsonPropertyName("content")] public OneBotSegment Content { get; set; } = new();
}

[Serializable]
public class OneBotFakeNodeText : OneBotFakeNodeBase
{
    [JsonPropertyName("content")] public string Content { get; set; } = "";
}

public class OneBotFakeNodeConverter<T, TList, TSimple, TText> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Utf8JsonReader readerClone = reader;
        while (readerClone.Read())
        {
            if (readerClone.TokenType != JsonTokenType.PropertyName) continue;
            if (readerClone.GetString() != "content") continue;
            readerClone.Read();
            switch (readerClone.TokenType)
            {
                case JsonTokenType.StartArray:
                    return (T?)JsonSerializer.Deserialize(ref reader, typeof(TList), options);
                case JsonTokenType.StartObject:
                    return (T?)JsonSerializer.Deserialize(ref reader, typeof(TSimple), options);
                case JsonTokenType.String:
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
