using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.File;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotGetFilesResponse(List<OneBotFile> files, List<OneBotFolder> folders)
{
    [JsonPropertyName("files")] public List<OneBotFile> Files { get; set; } = files;

    [JsonPropertyName("folders")] public List<OneBotFolder> Folders { get; set; } = folders;
}