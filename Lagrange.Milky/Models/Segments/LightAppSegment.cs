using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class LightAppIncomingSegment : IncomingSegmentBase<LightAppIncomingSegmentData>;
public class LightAppIncomingSegmentData
{
    [JsonPropertyName("app_name")] public required string AppName { get; init; }
    [JsonPropertyName("json_payload")] public required string JsonPayload { get; init; }
}

public sealed class LightAppOutgoingSegment : OutgoingSegmentBase<LightAppOutgoingSegmentData>;
public sealed class LightAppOutgoingSegmentData(string jsonPayload) {
    [JsonPropertyName("json_payload")] public required string JsonPayload { get; init; } = jsonPayload;
}