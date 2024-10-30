using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Entity.Common;
#pragma warning disable CS8618

[Serializable]
public class LightApp
{
    [JsonPropertyName("app")] public string App { get; set; }

    [JsonPropertyName("config")] public Config Config { get; set; }

    [JsonPropertyName("desc")] public string Desc { get; set; }

    [JsonPropertyName("from")] public long From { get; set; }

    [JsonPropertyName("meta")] [JsonConverter(typeof(NullIfEmptyConverter<Meta>))] public Meta Meta { get; set; }
    
    [JsonPropertyName("extra")] [JsonConverter(typeof(NullIfEmptyConverter<Extra>))] public Extra? Extra { get; set; }

    [JsonPropertyName("prompt")] public string Prompt { get; set; }

    [JsonPropertyName("ver")] public string Ver { get; set; }

    [JsonPropertyName("view")] public string View { get; set; }
    
    [JsonPropertyName("bizsrc")] public string BizSrc { get; set; }
}

[Serializable]
public class Config
{
    [JsonPropertyName("autosize")] public bool Autosize { get; set; }
    
    [JsonPropertyName("ctime")] public long Ctime { get; set; }
    
    [JsonPropertyName("token")] public string Token { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }
    
    [JsonPropertyName("forward")] public int Forward { get; set; }
    
    [JsonPropertyName("height")] public int Height { get; set; }
    
    [JsonPropertyName("width")] public int Width { get; set; }
    
    [JsonPropertyName("showsender")] public int ShowSender { get; set; }
}

[Serializable]
public class Meta
{
    [JsonPropertyName("Location.Search")] public LocationSearch LocationSearch { get; set; }
    
    [JsonPropertyName("pic")] public Pic Pic { get; set; }
    
    [JsonPropertyName("music")] public Music Music { get; set; }
}

[Serializable]
public class Extra
{
    [JsonPropertyName("app_type")] public int AppType { get; set; }

    [JsonPropertyName("appid")] public int AppId { get; set; }

    [JsonPropertyName("uin")] public uint Uin { get; set; }
}