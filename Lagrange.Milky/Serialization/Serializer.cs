using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Common;
using Lagrange.Milky.Models;
using Lagrange.Milky.Models.Events;
using Lagrange.Milky.Models.Messages;
using Lagrange.Milky.Models.Segments;
using Lagrange.Milky.Signing;

namespace Lagrange.Milky.Serialization;

public static partial class Serializer
{
    public static string JsonSerialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, typeof(T), JsonContext.Default);
    }

    public static byte[] JsonSerializeToUtf8Bytes<T>(T value)
    {
        return JsonSerializer.SerializeToUtf8Bytes(value, typeof(T), JsonContext.Default);
    }

    public static T? JsonDeserialize<T>(string json)
    {
        return (T?)JsonSerializer.Deserialize(json, typeof(T), JsonContext.Default);
    }

    public static ValueTask<T?> JsonDeserializeAsync<T>(Stream stream, CancellationToken ct = default)
    {
        var vt = JsonSerializer.DeserializeAsync(stream, typeof(T), JsonContext.Default, ct);
        return vt.IsCompleted ? ValueTask.FromResult((T?)vt.GetAwaiter().GetResult()) : CastAwait(vt);

        static async ValueTask<T?> CastAwait(ValueTask<object?> vt) => (T?)await vt;
    }

    // Signer
    [JsonSerializable(typeof(BotKeystore))]
    [JsonSerializable(typeof(SecSignRequest))]
    [JsonSerializable(typeof(SignerResponse<SecSignResult>))]
    // Event
    [JsonSerializable(typeof(MilkyEvent))]
    [JsonSerializable(typeof(BotOfflineEventData))]
    [JsonSerializable(typeof(MessageRecallEventData))]
    [JsonSerializable(typeof(FriendRequestEventData))]
    [JsonSerializable(typeof(GroupJoinRequestEventData))]
    [JsonSerializable(typeof(GroupInvitedJoinRequestEventData))]
    [JsonSerializable(typeof(GroupInvitationEventData))]
    [JsonSerializable(typeof(FriendNudgeEventData))]
    [JsonSerializable(typeof(GroupMemberIncreaseEventData))]
    [JsonSerializable(typeof(GroupMemberDecreaseEventData))]
    [JsonSerializable(typeof(GroupMessageReactionEventData))]
    [JsonSerializable(typeof(GroupMuteEventData))]
    [JsonSerializable(typeof(GroupNudgeEventData))]
    // Message
    [JsonSerializable(typeof(FriendIncomingMessage))]
    [JsonSerializable(typeof(GroupIncomingMessage))]
    [JsonSerializable(typeof(TempIncomingMessage))]
    // Segment
    [JsonSerializable(typeof(TextIncomingSegmentData))]
    [JsonSerializable(typeof(MentionIncomingSegment))]
    private sealed partial class JsonContext : JsonSerializerContext;
}