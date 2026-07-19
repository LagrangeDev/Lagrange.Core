using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class FriendRequestEventConverter : IEventConverter<BotFriendRequestEvent, FriendRequestEventConverter.Data>
{
    public string Name => "friend_request";

    public bool CanConvert(BotFriendRequestEvent @event) => true;

    public ValueTask<Data> ConvertAsync(BotFriendRequestEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        InitiatorId = @event.InitiatorUin,
        InitiatorUid = string.Empty, // We don't support operations; if it's just for viewing, Uin is sufficient.
        Comment = @event.Message,
        Via = @event.Source
    });

    public class Data
    {
        [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
        [JsonPropertyName("initiator_uid")] public required string InitiatorUid { get; init; }
        [JsonPropertyName("comment")] public required string Comment { get; init; }
        [JsonPropertyName("via")] public required string Via { get; init; }
    }
}