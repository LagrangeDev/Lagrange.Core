using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Messages;

public class TempIncomingMessage : IncomingMessageBase
{
    // public override string MessageScene => "group";

    [JsonPropertyName("group")] public required Group? Group { get; init; }
}