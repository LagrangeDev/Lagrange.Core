using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Common;
#pragma warning disable CS8618

[Serializable]
public class Music
{
    [JsonPropertyName("action")] public string Action { get; set; }

    [JsonPropertyName("android_pkg_name")] public string AndroidPkgName { get; set; }

    [JsonPropertyName("app_type")] public int AppType { get; set; }

    [JsonPropertyName("appid")] public int AppId { get; set; }

    [JsonPropertyName("ctime")] public int Ctime { get; set; }

    [JsonPropertyName("desc")] public string Desc { get; set; }

    [JsonPropertyName("jumpUrl")] public string JumpUrl { get; set; }

    [JsonPropertyName("musicUrl")] public string MusicUrl { get; set; }

    [JsonPropertyName("preview")] public string Preview { get; set; }

    [JsonPropertyName("sourceMsgId")] public string SourceMsgId { get; set; }

    [JsonPropertyName("source_icon")] public string SourceIcon { get; set; }

    [JsonPropertyName("source_url")] public string SourceUrl { get; set; }

    [JsonPropertyName("tag")] public string Tag { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("uin")] public uint Uin { get; set; }
}