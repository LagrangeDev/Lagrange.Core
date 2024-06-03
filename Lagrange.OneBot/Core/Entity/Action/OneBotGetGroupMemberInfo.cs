using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetGroupMemberInfo
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("no_cache")] public bool NoCache { get; set; }
}
