using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotFriendPoke
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
}