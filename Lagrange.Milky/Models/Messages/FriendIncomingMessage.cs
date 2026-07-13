using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Messages;

public class FriendIncomingMessage : IncomingMessageBase
{
    // public override string MessageScene => "friend";

    [JsonPropertyName("friend")] public required Friend Friend { get; init; }
}
