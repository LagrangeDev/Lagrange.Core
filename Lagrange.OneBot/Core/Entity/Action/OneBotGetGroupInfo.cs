using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetGroupInfo
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("no_cache")] public bool NoCache { get; set; }
}
