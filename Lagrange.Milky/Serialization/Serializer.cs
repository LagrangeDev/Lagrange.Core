using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Common;
using Lagrange.Milky.Api;
using Lagrange.Milky.Api.Handlers.File;
using Lagrange.Milky.Api.Handlers.Friend;
using Lagrange.Milky.Api.Handlers.Group;
using Lagrange.Milky.Api.Handlers.Message;
using Lagrange.Milky.Api.Handlers.System;
using Lagrange.Milky.Events;
using Lagrange.Milky.Events.Converters;
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
    public static Task JsonSerializableAsync<T>(Stream stream, T value, CancellationToken ct)
    {
        return JsonSerializer.SerializeAsync(stream, value, typeof(T), JsonContext.Default, ct);
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
    public static ValueTask<object?> JsonDeserializeAsync(Stream stream, Type type, CancellationToken ct)
    {
        return JsonSerializer.DeserializeAsync(stream, type, JsonContext.Default, ct);
    }

    // Signer
    [JsonSerializable(typeof(BotKeystore))]
    [JsonSerializable(typeof(SecSignRequest))]
    [JsonSerializable(typeof(SignerResponse<SecSignResult>))]
    // Api
    [JsonSerializable(typeof(MilkyApiResponse))]
    [JsonSerializable(typeof(GetLoginInfoHandler.Result), TypeInfoPropertyName = "GetLoginInfoResult")]
    [JsonSerializable(typeof(GetImplInfoHandler.Result), TypeInfoPropertyName = "GetImplInfoResult")]
    [JsonSerializable(typeof(GetUserProfileHandler.Request), TypeInfoPropertyName = "GetUserProfileRequest")]
    [JsonSerializable(typeof(GetUserProfileHandler.Result), TypeInfoPropertyName = "GetUserProfileResult")]
    [JsonSerializable(typeof(GetFriendListHandler.Request), TypeInfoPropertyName = "GetFriendListRequest")]
    [JsonSerializable(typeof(GetFriendListHandler.Result), TypeInfoPropertyName = "GetFriendListResult")]
    [JsonSerializable(typeof(GetFriendInfoHandler.Request), TypeInfoPropertyName = "GetFriendInfoRequest")]
    [JsonSerializable(typeof(GetFriendInfoHandler.Result), TypeInfoPropertyName = "GetFriendInfoResult")]
    [JsonSerializable(typeof(GetGroupListHandler.Request), TypeInfoPropertyName = "GetGroupListRequest")]
    [JsonSerializable(typeof(GetGroupListHandler.Result), TypeInfoPropertyName = "GetGroupListResult")]
    [JsonSerializable(typeof(GetGroupInfoHandler.Request), TypeInfoPropertyName = "GetGroupInfoRequest")]
    [JsonSerializable(typeof(GetGroupInfoHandler.Result), TypeInfoPropertyName = "GetGroupInfoResult")]
    [JsonSerializable(typeof(GetGroupMemberListHandler.Request), TypeInfoPropertyName = "GetGroupMemberListRequest")]
    [JsonSerializable(typeof(GetGroupMemberListHandler.Result), TypeInfoPropertyName = "GetGroupMemberListResult")]
    [JsonSerializable(typeof(GetGroupMemberInfoHandler.Request), TypeInfoPropertyName = "GetGroupMemberInfoRequest")]
    [JsonSerializable(typeof(GetGroupMemberInfoHandler.Result), TypeInfoPropertyName = "GetGroupMemberInfoResult")]
    [JsonSerializable(typeof(SetPeerPinHandler.Request), TypeInfoPropertyName = "SetPeerPinRequest")]
    [JsonSerializable(typeof(SetAvatarHandler.Request), TypeInfoPropertyName = "SetAvatarRequest")]
    [JsonSerializable(typeof(GetCookiesHandler.Request), TypeInfoPropertyName = "GetCookiesRequest")]
    [JsonSerializable(typeof(GetCookiesHandler.Result), TypeInfoPropertyName = "GetCookiesResult")]
    [JsonSerializable(typeof(SendPrivateMessageHandler.Request), TypeInfoPropertyName = "SendPrivateMessageRequest")]
    [JsonSerializable(typeof(SendPrivateMessageHandler.Result), TypeInfoPropertyName = "SendPrivateMessageResult")]
    [JsonSerializable(typeof(SendGroupMessageHandler.Request), TypeInfoPropertyName = "SendGroupMessageRequest")]
    [JsonSerializable(typeof(SendGroupMessageHandler.Result), TypeInfoPropertyName = "SendGroupMessageResult")]
    [JsonSerializable(typeof(RecallPrivateMessageHandler.Request), TypeInfoPropertyName = "RecallPrivateMessageRequest")]
    [JsonSerializable(typeof(RecallGroupMessageHandler.Request), TypeInfoPropertyName = "RecallGroupMessageRequest")]
    [JsonSerializable(typeof(GetMessageHandler.Request), TypeInfoPropertyName = "GetMessageRequest")]
    [JsonSerializable(typeof(GetMessageHandler.Result), TypeInfoPropertyName = "GetMessageResult")]
    [JsonSerializable(typeof(GetHistoryMessagesHandler.Request), TypeInfoPropertyName = "GetHistoryMessagesRequest")]
    [JsonSerializable(typeof(GetHistoryMessagesHandler.Result), TypeInfoPropertyName = "GetHistoryMessagesResult")]
    [JsonSerializable(typeof(GetResourceTempUrlHandler.Request), TypeInfoPropertyName = "GetResourceTempUrlRequest")]
    [JsonSerializable(typeof(GetResourceTempUrlHandler.Result), TypeInfoPropertyName = "GetResourceTempUrlResult")]
    [JsonSerializable(typeof(SendFriendNudgeHandler.Request), TypeInfoPropertyName = "SendFriendNudgeRequest")]
    [JsonSerializable(typeof(SetGroupNameHandler.Request), TypeInfoPropertyName = "SetGroupNameRequest")]
    [JsonSerializable(typeof(SetGroupMemberCardHandler.Request), TypeInfoPropertyName = "SetGroupMemberCardRequest")]
    [JsonSerializable(typeof(SetGroupMemberSpecialTitleHandler.Request), TypeInfoPropertyName = "SetGroupMemberSpecialTitleRequest")]
    [JsonSerializable(typeof(SetGroupWholeMuteHandler.Request), TypeInfoPropertyName = "SetGroupWholeMuteRequest")]
    [JsonSerializable(typeof(KickGroupMemberHandler.Request), TypeInfoPropertyName = "KickGroupMemberRequest")]
    [JsonSerializable(typeof(SetGroupEssenceMessageHandler.Request), TypeInfoPropertyName = "SetGroupEssenceMessageRequest")]
    [JsonSerializable(typeof(QuitGroupHandler.Request), TypeInfoPropertyName = "QuitGroupRequest")]
    [JsonSerializable(typeof(SendGroupMessageReactionHandler.Request), TypeInfoPropertyName = "SendGroupMessageReactionRequest")]
    [JsonSerializable(typeof(SendGroupNudgeHandler.Request), TypeInfoPropertyName = "SendGroupNudgeRequest")]
    [JsonSerializable(typeof(GetGroupNotificationsHandler.Request), TypeInfoPropertyName = "GetGroupNotificationsRequest")]
    [JsonSerializable(typeof(GetGroupNotificationsHandler.Result), TypeInfoPropertyName = "GetGroupNotificationsResult")]
    [JsonSerializable(typeof(AcceptGroupRequestHandler.Request), TypeInfoPropertyName = "AcceptGroupRequestRequest")]
    [JsonSerializable(typeof(RejectGroupRequestHandler.Request), TypeInfoPropertyName = "RejectGroupRequestRequest")]
    [JsonSerializable(typeof(AcceptGroupInvitationHandler.Request), TypeInfoPropertyName = "AcceptGroupInvitationRequest")]
    [JsonSerializable(typeof(RejectGroupInvitationHandler.Request), TypeInfoPropertyName = "RejectGroupInvitationRequest")]
    [JsonSerializable(typeof(UploadPrivateFileHandler.Request), TypeInfoPropertyName = "UploadPrivateFileRequest")]
    [JsonSerializable(typeof(UploadPrivateFileHandler.Result), TypeInfoPropertyName = "UploadPrivateFileResult")]
    [JsonSerializable(typeof(UploadGroupFileHandler.Request), TypeInfoPropertyName = "UploadGroupFileRequest")]
    [JsonSerializable(typeof(UploadGroupFileHandler.Result), TypeInfoPropertyName = "UploadGroupFileResult")]
    [JsonSerializable(typeof(GetGroupFileDownloadUrlHandler.Request), TypeInfoPropertyName = "GetGroupFileDownloadUrlRequest")]
    [JsonSerializable(typeof(GetGroupFileDownloadUrlHandler.Result), TypeInfoPropertyName = "GetGroupFileDownloadUrlResult")]
    [JsonSerializable(typeof(GetGroupFilesHandler.Request), TypeInfoPropertyName = "GetGroupFilesRequest")]
    [JsonSerializable(typeof(GetGroupFilesHandler.Result), TypeInfoPropertyName = "GetGroupFilesResult")]
    [JsonSerializable(typeof(MoveGroupFileHandler.Request), TypeInfoPropertyName = "MoveGroupFileRequest")]
    [JsonSerializable(typeof(DeleteGroupFileHandler.Request), TypeInfoPropertyName = "DeleteGroupFileRequest")]
    [JsonSerializable(typeof(CreateGroupFolderHandler.Request), TypeInfoPropertyName = "CreateGroupFolderRequest")]
    [JsonSerializable(typeof(CreateGroupFolderHandler.Result), TypeInfoPropertyName = "CreateGroupFolderResult")]
    [JsonSerializable(typeof(RenameGroupFolderHandler.Request), TypeInfoPropertyName = "RenameGroupFolderRequest")]
    [JsonSerializable(typeof(DeleteGroupFolderHandler.Request), TypeInfoPropertyName = "DeleteGroupFolderRequest")]
    // Event
    [JsonSerializable(typeof(MilkyEvent))]
    [JsonSerializable(typeof(BotOfflineEventConverter.Data), TypeInfoPropertyName = "BotOfflineEventData")]
    [JsonSerializable(typeof(MessageRecallEventConverter.Data), TypeInfoPropertyName = "MessageRecallEventData")]
    [JsonSerializable(typeof(FriendRequestEventConverter.Data), TypeInfoPropertyName = "FriendRequestEventData")]
    [JsonSerializable(typeof(GroupJoinRequestEventConverter.Data), TypeInfoPropertyName = "GroupJoinRequestEventData")]
    [JsonSerializable(typeof(GroupInvitedJoinRequestEventConverter.Data), TypeInfoPropertyName = "GroupInvitedJoinRequestEventData")]
    [JsonSerializable(typeof(GroupInvitationEventConverter.Data), TypeInfoPropertyName = "GroupInvitationEventData")]
    [JsonSerializable(typeof(GroupMemberIncreaseEventConverter.Data), TypeInfoPropertyName = "GroupMemberIncreaseEventData")]
    [JsonSerializable(typeof(GroupMemberDecreaseEventConverter.Data), TypeInfoPropertyName = "GroupMemberDecreaseEventData")]
    [JsonSerializable(typeof(GroupMessageReactionEventConverter.Data), TypeInfoPropertyName = "GroupMessageReactionEventData")]
    [JsonSerializable(typeof(GroupNudgeEventConverter.Data), TypeInfoPropertyName = "GroupNudgeEventData")]
    private sealed partial class JsonContext : JsonSerializerContext;
}