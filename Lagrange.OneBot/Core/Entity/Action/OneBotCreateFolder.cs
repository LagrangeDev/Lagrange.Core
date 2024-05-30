using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotCreateFolder
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("parent_id")] public string ParentId { get; set; } = string.Empty;
}