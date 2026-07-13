using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class FriendRequestEventConverter : IEventConverter<BotFriendRequestEvent, FriendRequestEventData>
{
    public string Name => "friend_request";

    public bool CanConvert(BotFriendRequestEvent @event) => true;

    public ValueTask<FriendRequestEventData> ConvertAsync(BotFriendRequestEvent @event, CancellationToken ct) => ValueTask.FromResult(new FriendRequestEventData
    {
        InitiatorId = @event.InitiatorUin,
        InitiatorUid = string.Empty, // We don't support operations; if it's just for viewing, Uin is sufficient.
        Comment = @event.Message,
        Via = @event.Source
    });
}