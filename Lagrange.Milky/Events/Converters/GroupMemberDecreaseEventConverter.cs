using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupMemberDecreaseEventConverter : IEventConverter<BotGroupMemberDecreaseEvent, GroupMemberDecreaseEventConverter.Data>
{
    public string Name => "group_member_decrease";

    public bool CanConvert(BotGroupMemberDecreaseEvent @event) => true;

    public ValueTask<Data> ConvertAsync(BotGroupMemberDecreaseEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        GroupId = @event.GroupUin,
        UserId = @event.UserUin,
        OperatorId = @event.OperatorUin,
    });

    public class Data
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; }
        [JsonPropertyName("user_id")] public required long UserId { get; init; }
        [JsonPropertyName("operator_id")] public required long? OperatorId { get; init; }
    }
}