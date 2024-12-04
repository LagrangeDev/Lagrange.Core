using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotPrivateFile(uint selfId, uint userId, OneBotPrivateFileInfo fileInfo) : OneBotNotify(selfId, "offline_file")
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("file")] public OneBotPrivateFileInfo Info { get; set; } = fileInfo;
}



[Serializable]

public class OneBotPrivateFileInfo(string id, string name, ulong size, string url, string hash)
{
    [JsonPropertyName("id")] public string Id { get; set; } = id;

    [JsonPropertyName("name")] public string Name { get; set; } = name;

    [JsonPropertyName("size")] public ulong Size { get; set; } = size;

    [JsonPropertyName("url")] public string Url { get; set; } = url;

    [JsonPropertyName("hash")] public string FileHash { get; set; } = hash;
}