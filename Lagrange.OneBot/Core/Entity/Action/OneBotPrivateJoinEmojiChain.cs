using System.Text.Json.Serialization;
namespace Lagrange.OneBot.Core.Entity.Action;

public class OneBotPrivateJoinEmojiChain
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("message_id")] public int MessageId { get; set; } 
    
    [JsonPropertyName("emoji_id")] public uint EmojiId { get; set; }
}