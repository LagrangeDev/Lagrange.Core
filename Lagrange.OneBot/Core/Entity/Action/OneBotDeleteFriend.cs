using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotDeleteFriend
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("block")] public bool Block { get; set; }
}