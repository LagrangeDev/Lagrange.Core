using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetGroupHonorInfo
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
}