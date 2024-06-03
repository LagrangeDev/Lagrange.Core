using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetFiles
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("folder_id")] public string? FolderId { get; set; }
}