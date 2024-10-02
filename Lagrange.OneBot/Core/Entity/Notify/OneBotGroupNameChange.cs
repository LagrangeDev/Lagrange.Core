using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupNameChange(uint selfId, uint groupId, string name) : OneBotNotify(selfId, "group_name_change")
{
    [JsonPropertyName("group_id")] public uint GroupId { get; } = groupId;

    [JsonPropertyName("name")] public string Name { get; } = name;
}