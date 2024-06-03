using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGroupForward : OneBotForward
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
}