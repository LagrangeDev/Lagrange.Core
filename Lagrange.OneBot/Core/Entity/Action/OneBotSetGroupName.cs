using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupName
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("group_name")] public string GroupName { get; set; } = "";
}