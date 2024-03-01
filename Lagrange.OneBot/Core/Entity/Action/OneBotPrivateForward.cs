using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotPrivateForward : OneBotForward
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
}