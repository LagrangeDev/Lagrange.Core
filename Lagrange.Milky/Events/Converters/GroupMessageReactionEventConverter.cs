using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupMessageReactionEventConverter : IEventConverter<BotGroupReactionEvent, GroupMessageReactionEventConverter.Data>
{
    public string Name => "group_message_reaction";

    public bool CanConvert(BotGroupReactionEvent @event) => true;

    public ValueTask<Data> ConvertAsync(BotGroupReactionEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        GroupId = @event.TargetGroupUin,
        UserId = @event.OperatorUin,
        MessageSeq = (long)@event.TargetSequence,
        FaceId = @event.Code,
        ReactionType = "face", // TODO: reaction type nudeg event is not implemented in core
        IsAdd = @event.IsAdd,
    });

    public class Data
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; }
        [JsonPropertyName("user_id")] public required long UserId { get; init; }
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
        [JsonPropertyName("face_id")] public required string FaceId { get; init; }
        [JsonPropertyName("reaction_type")] public required string ReactionType { get; init; }
        [JsonPropertyName("is_add")] public required bool IsAdd { get; init; }
    }
}