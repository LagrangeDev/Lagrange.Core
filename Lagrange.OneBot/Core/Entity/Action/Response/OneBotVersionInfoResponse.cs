using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotVersionInfoResponse(string ntProtocol)
{
    [JsonPropertyName("app_name")] public string AppName => Constant.OneBotImpl;

    [JsonPropertyName("app_version")] public string AppVersion => Constant.OneBotImplVersion;

    [JsonPropertyName("protocol_version")] public string ProtocolVersion => $"v{Constant.OneBotProtocolVersion}";

    [JsonPropertyName("nt_protocol")] public string NTProtocol { get; } = ntProtocol;
}