using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupNudgeEventConverter : IEventConverter<BotGroupNudgeEvent, GroupNudgeEventConverter.Data>
{
    public string Name => "group_nudge";

    public bool CanConvert(BotGroupNudgeEvent @event) => true;

    public ValueTask<Data> ConvertAsync(BotGroupNudgeEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        GroupId = @event.GroupUin,
        SenderId = @event.OperatorUin,
        ReceiverId = @event.TargetUin,
        DisplayAction = @event.Action,
        DisplaySuffix = @event.Suffix,
        DisplayActionImgUrl = @event.ActionImageUrl,
    });

    public class Data
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; }
        [JsonPropertyName("sender_id")] public required long SenderId { get; init; }
        [JsonPropertyName("receiver_id")] public required long ReceiverId { get; init; }
        [JsonPropertyName("display_action")] public required string DisplayAction { get; init; }
        [JsonPropertyName("display_suffix")] public required string DisplaySuffix { get; init; }
        [JsonPropertyName("display_action_img_url")] public required string DisplayActionImgUrl { get; init; }
    }
}