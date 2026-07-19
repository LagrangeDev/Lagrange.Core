using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("set_peer_pin")]
public sealed class SetPeerPinHandler(BotContext lagrange) : INoResultApiHandler<SetPeerPinHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await (request.MessageScene switch
        {
            "friend" => _lagrange.SetPinFriend(request.PeerId, request.IsPinned).WaitAsync(ct),
            "group" => _lagrange.SetPinGroup(request.PeerId, request.IsPinned).WaitAsync(ct),
            _ => throw new NotSupportedException(),
        });
        return new MilkyApiResponse();
    }

    public sealed class Request(string messageScene, long peerId, bool isPinned = true)
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; } = messageScene;
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; } = peerId;
        [JsonPropertyName("is_pinned")] public bool IsPinned { get; init; } = isPinned;
    }
}
