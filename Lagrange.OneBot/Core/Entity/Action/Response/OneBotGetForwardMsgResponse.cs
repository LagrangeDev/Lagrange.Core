using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotGetForwardMsgResponse(List<OneBotSegment> message)
{
    [JsonPropertyName("message")] public List<OneBotSegment> Message { get; set; } = message;
}