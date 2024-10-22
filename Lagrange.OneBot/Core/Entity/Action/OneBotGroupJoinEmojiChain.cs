using System.Text.Json.Serialization;
namespace Lagrange.OneBot.Core.Entity.Action;

public class OneBotGroupJoinEmojiChain
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("message_id")] public int MessageId { get; set; } 
    
    [JsonPropertyName("emoji_id")] public uint EmojiId { get; set; }
}