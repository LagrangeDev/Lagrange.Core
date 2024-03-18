using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotPrivateFile(uint selfId, uint userId, OneBotFileInfo fileInfo) : OneBotNotify(selfId, "offline_file")
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("file")] public OneBotFileInfo Info { get; set; } = fileInfo;
}