using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupFile(uint selfId, uint groupId, uint userId, OneBotFileInfo fileInfo) : OneBotNotify(selfId, "group_upload")
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;

    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("file")] public OneBotFileInfo Info { get; set; } = fileInfo;
}

[Serializable]
public class OneBotFileInfo(string id, string name, ulong size)
{
    [JsonPropertyName("id")] public string Id { get; set; } = id;

    [JsonPropertyName("name")] public string Name { get; set; } = name;

    [JsonPropertyName("size")] public ulong Size { get; set; } = size;
    
    [JsonPropertyName("busid")] public uint BusId { get; set; }
}