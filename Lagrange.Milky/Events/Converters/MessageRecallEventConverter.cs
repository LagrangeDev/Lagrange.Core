using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

public class MessageRecallEventConverter
{
    [EventConverter]
    public class Friend : IEventConverter<BotFriendRecallEvent, Data>
    {
        public string Name => "message_recall";

        public bool CanConvert(BotFriendRecallEvent @event) => true;

        public ValueTask<Data> ConvertAsync(BotFriendRecallEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
        {
            MessageScene = "friend",
            PeerId = @event.PeerUin,
            MessageSeq = (long)@event.Sequence,
            SenderId = @event.AuthorUin,
            OperatorId = @event.AuthorUin,
            DisplaySuffix = @event.Tip,
        });
    }

    [EventConverter]
    public class Group : IEventConverter<BotGroupRecallEvent, Data>
    {
        public string Name => "message_recall";

        public bool CanConvert(BotGroupRecallEvent @event) => true;

        public ValueTask<Data> ConvertAsync(BotGroupRecallEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
        {
            MessageScene = "group",
            PeerId = @event.GroupUin,
            MessageSeq = (long)@event.Sequence,
            SenderId = @event.AuthorUin,
            OperatorId = @event.OperatorUin,
            DisplaySuffix = @event.Tip,
        });
    }

    public class Data
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; }
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; }
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
        [JsonPropertyName("sender_id")] public required long SenderId { get; init; }
        [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
        [JsonPropertyName("display_suffix")] public required string DisplaySuffix { get; init; }
    }
}