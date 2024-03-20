using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetGroupMemo
{
    [JsonPropertyName("group_id")] public long GroupId { get; set; }
}
