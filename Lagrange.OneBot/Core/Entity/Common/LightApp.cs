using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Common;
#pragma warning disable CS8618

[Serializable]
public class LightApp
{
    [JsonPropertyName("app")] public string App { get; set; }

    [JsonPropertyName("config")] public Config Config { get; set; }

    [JsonPropertyName("desc")] public string Desc { get; set; }

    [JsonPropertyName("from")] public long From { get; set; }

    [JsonPropertyName("meta")] public Meta Meta { get; set; }

    [JsonPropertyName("prompt")] public string Prompt { get; set; }

    [JsonPropertyName("ver")] public string Ver { get; set; }

    [JsonPropertyName("view")] public string View { get; set; }
}

[Serializable]
public class Config
{
    [JsonPropertyName("autosize")] public bool Autosize { get; set; }
    
    [JsonPropertyName("ctime")] public long Ctime { get; set; }
    
    [JsonPropertyName("token")] public string Token { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }
}

[Serializable]
public class Meta
{
    [JsonPropertyName("Location.Search")] public LocationSearch LocationSearch { get; set; }
}