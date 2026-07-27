using System.Collections.Generic;
using System.Text.Json.Serialization;
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Models.Segments;

public class ForwardIncomingSegment : IncomingSegmentBase<ForwardIncomingSegmentData>;
public class ForwardIncomingSegmentData
{
    [JsonPropertyName("forward_id")] public required string ForwardId { get; init; }
    [JsonPropertyName("title")] public required string Title { get; init; }
    [JsonPropertyName("preview")] public required IReadOnlyList<string> Preview { get; init; }
    [JsonPropertyName("summary")] public required string Summary { get; init; }
}


public sealed class ForwardOutgoingSegment : OutgoingSegmentBase<ForwardOutgoingSegmentData>;
public sealed class ForwardOutgoingSegmentData(IReadOnlyList<OutgoingForwardedMessage> messages, string? title, IReadOnlyList<string>? preview, string? summary, string? prompt)
{
    [JsonPropertyName("messages")] public required IReadOnlyList<OutgoingForwardedMessage> Messages { get; init; } = messages;
    [JsonPropertyName("title")] public string? Title { get; init; } = title;
    [JsonPropertyName("preview")] public IReadOnlyList<string>? Preview { get; init; } = preview;
    [JsonPropertyName("summary")] public string? Summary { get; init; } = summary;
    [JsonPropertyName("prompt")] public string? Prompt { get; init; } = prompt;
}