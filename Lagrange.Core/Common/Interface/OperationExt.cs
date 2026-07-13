using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Response;
using Lagrange.Core.Internal.Logic;

namespace Lagrange.Core.Common.Interface;

public static class OperationExt
{
    public static Task<BotQrCodeInfo?> FetchQrCodeInfo(this BotContext context, byte[] k) =>
        context.EventContext.GetLogic<WtExchangeLogic>().FetchQrCodeInfo(k);

    public static Task<(bool Success, string Message)> CloseQrCode(this BotContext context, byte[] k, bool confirm) =>
        context.EventContext.GetLogic<WtExchangeLogic>().CloseQrCode(k, confirm);

    public static Task<Dictionary<string, string>> FetchCookies(this BotContext context, params List<string> domains) =>
        context.EventContext.GetLogic<OperationLogic>().FetchCookies(domains);

    public static Task<(string Key, uint Expiration)> FetchClientKey(this BotContext context) =>
        context.EventContext.GetLogic<OperationLogic>().FetchClientKey();

    public static Task<bool> SetStatus(this BotContext context, uint status) =>
        context.EventContext.GetLogic<OperationLogic>().SetStatus(status);

    public static Task<bool> SetCustomStatus(this BotContext context, uint faceId, string text) =>
        context.EventContext.GetLogic<OperationLogic>().SetCustomStatus(faceId, text);

    public static Task GroupRecallPoke(this BotContext context, ulong groupUin, ulong messageSequence, ulong messageTime, ulong tipsSeqId) => 
        context.EventContext.GetLogic<OperationLogic>().GroupRecallPoke(groupUin, messageSequence, messageTime, tipsSeqId);

    public static Task FriendRecallPoke(this BotContext context, ulong peerUin, ulong messageSequence, ulong messageTime, ulong tipsSeqId) => 
        context.EventContext.GetLogic<OperationLogic>().FriendRecallPoke(peerUin, messageSequence, messageTime, tipsSeqId);

    public static Task<List<BotFriend>> FetchFriends(this BotContext context, bool refresh = false) =>
        context.CacheContext.GetFriendList(refresh);

    public static Task<List<BotGroup>> FetchGroups(this BotContext context, bool refresh = false) =>
        context.CacheContext.GetGroupList(refresh);

    public static Task<BotGroupExtra> FetchGroupExtra(this BotContext context, long groupUin) =>
        context.EventContext.GetLogic<OperationLogic>().FetchGroupExtra(groupUin);

    public static Task<BotStrangerGroupInfo> FetchStrangerGroupInfo(this BotContext context, ulong groupUin) =>
        context.EventContext.GetLogic<OperationLogic>().FetchStrangerGroupInfo(groupUin);

    public static Task<List<BotGroupMember>> FetchMembers(this BotContext context, long groupUin, bool refresh = false) =>
        context.CacheContext.GetMemberList(groupUin, refresh);

    public static Task<List<BotGroupNotificationBase>> FetchGroupNotifications(this BotContext context, ulong count, ulong start = 0) =>
        context.EventContext.GetLogic<OperationLogic>().FetchGroupNotifications(count, start);

    public static Task<List<BotGroupNotificationBase>> FetchFilteredGroupNotifications(this BotContext context, ulong count, ulong start = 0) =>
        context.EventContext.GetLogic<OperationLogic>().FetchFilteredGroupNotifications(count, start);

    public static Task<BotStranger> FetchStranger(this BotContext context, long uin) =>
        context.EventContext.GetLogic<OperationLogic>().FetchStranger(uin);

    public static Task SetGroupNotification(this BotContext context, long groupUin, ulong sequence, BotGroupNotificationType type, bool isFiltered, GroupNotificationOperate operate, string message = "") =>
        context.EventContext.GetLogic<OperationLogic>().SetGroupNotification(groupUin, sequence, type, isFiltered, operate, message);

    public static Task SetGroupReaction(this BotContext context, long groupUin, ulong sequence, string code, bool isAdd) =>
        context.EventContext.GetLogic<OperationLogic>().SetGroupReaction(groupUin, sequence, code, isAdd);

    public static Task SetGroupTodo(this BotContext context, long groupUin, ulong sequence) =>
        context.EventContext.GetLogic<OperationLogic>().SetGroupTodo(groupUin, sequence);

    public static Task FinishGroupTodo(this BotContext context, long groupUin) =>
        context.EventContext.GetLogic<OperationLogic>().FinishGroupTodo(groupUin);

    public static Task RemoveGroupTodo(this BotContext context, long groupUin) =>
        context.EventContext.GetLogic<OperationLogic>().RemoveGroupTodo(groupUin);

    public static Task SetPinFriend(this BotContext context, long friendUin, bool isPin) =>
        context.EventContext.GetLogic<OperationLogic>().SetPinFriend(friendUin, isPin);

    public static Task SetPinGroup(this BotContext context, long groupUin, bool isPin) =>
        context.EventContext.GetLogic<OperationLogic>().SetPinGroup(groupUin, isPin);

    public static Task<string> GetNTV2RichMediaUrl(this BotContext context, string fileUuid) =>
        context.EventContext.GetLogic<OperationLogic>().GetNTV2RichMediaUrl(fileUuid);

    public static Task<bool> SetBotAvatar(this BotContext context, Stream stream) =>
        context.HighwayContext.UploadFile(stream, 90, Array.Empty<byte>());
}
