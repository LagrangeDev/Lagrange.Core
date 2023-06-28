using System.Text.Json.Serialization;
using MessagePack;

namespace Lagrange.OneBot.Core.Entity.Generic;

[Serializable]
[MessagePackObject]
internal class OneBotVersion
{
    [JsonPropertyName("impl")] [Key("impl")] public string Impl { get; set; } = Constant.OneBotImpl;
    
    [JsonPropertyName("version")] [Key("version")] public string Version { get; set; } = Constant.OneBotImplVersion;
    
    [JsonPropertyName("onebot_version")] [Key("onebot_version")] public int OneBotVersionNumber { get; set; } = Constant.OneBotProtocolVersion;
}