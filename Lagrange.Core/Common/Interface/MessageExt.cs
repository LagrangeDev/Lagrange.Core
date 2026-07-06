using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Logic;
using Lagrange.Core.Message;

namespace Lagrange.Core.Common.Interface;

public static class MessageExt
{
    public static Task<BotMessage> SendFriendMessage(this BotContext context, long friendUin, MessageChain chain)
        => context.EventContext.GetLogic<MessagingLogic>().SendFriendMessage(friendUin, chain);

    public static Task<BotMessage> SendGroupMessage(this BotContext context, long groupUin, MessageChain chain)
        => context.EventContext.GetLogic<MessagingLogic>().SendGroupMessage(groupUin, chain);

    public static Task<List<BotMessage>> GetGroupMessage(this BotContext context, long groupUin, ulong startSequence, ulong endSequence)
        => context.EventContext.GetLogic<MessagingLogic>().GetGroupMessage(groupUin, startSequence, endSequence);

    public static Task<List<BotMessage>> GetRoamMessage(this BotContext context, long friendUin, uint timestamp, uint count)
        => context.EventContext.GetLogic<MessagingLogic>().GetRoamMessage(friendUin, timestamp, count);

    public static Task<List<BotMessage>> GetRoamMessage(this BotContext context, BotMessage target, uint count)
    {
        uint timestamp = (uint)new DateTimeOffset(target.Time).ToUnixTimeSeconds();
        return context.EventContext.GetLogic<MessagingLogic>().GetRoamMessage(target.Contact.Uin, timestamp, count);
    }

    public static Task<List<BotMessage>> GetC2CMessage(this BotContext context, long peerUin, ulong startSequence, ulong endSequence)
        => context.EventContext.GetLogic<MessagingLogic>().GetC2CMessage(peerUin, startSequence, endSequence);

    public static Task<(ulong Sequence, DateTime Time)> SendFriendFile(this BotContext context, long targetUin, Stream fileStream, string? fileName = null)
        => context.EventContext.GetLogic<OperationLogic>().SendFriendFile(targetUin, fileStream, fileName);

    public static Task<string> SendGroupFile(this BotContext context, long groupUin, Stream fileStream, string? fileName = null, string parentDirectory = "/")
        => context.EventContext.GetLogic<OperationLogic>().SendGroupFile(groupUin, fileStream, fileName, parentDirectory);

    public static Task RecallMessage(this BotContext context, BotMessage message)
        => context.EventContext.GetLogic<MessagingLogic>().RecallMessage(message);

    public static Task SetEssenceMessage(this BotContext context, BotMessage message)
        => context.EventContext.GetLogic<MessagingLogic>().SetEssenceMessage(message);

    public static Task SetEssenceMessage(this BotContext context, long groupUin, ulong sequence, uint random)
        => context.EventContext.GetLogic<MessagingLogic>().SetEssenceMessage(groupUin, sequence, random);

    public static Task RemoveEssenceMessage(this BotContext context, BotMessage message)
        => context.EventContext.GetLogic<MessagingLogic>().RemoveEssenceMessage(message);

    public static Task RemoveEssenceMessage(this BotContext context, long groupUin, ulong sequence, uint random)
        => context.EventContext.GetLogic<MessagingLogic>().RemoveEssenceMessage(groupUin, sequence, random);

    public static Task<string> GroupFSDownload(this BotContext context, long groupUin, string fileId)
        => context.EventContext.GetLogic<OperationLogic>().GroupFSDownload(groupUin, fileId);

    public static Task<ulong> FetchGroupFSSpace(this BotContext context, long groupUin)
        => context.EventContext.GetLogic<OperationLogic>().FetchGroupFSSpace(groupUin);

    public static Task<uint> FetchGroupFSCount(this BotContext context, long groupUin)
        => context.EventContext.GetLogic<OperationLogic>().FetchGroupFSCount(groupUin);

    public static Task<List<IBotFSEntry>> FetchGroupFSList(this BotContext context, long groupUin, string targetDirectory = "/")
        => context.EventContext.GetLogic<OperationLogic>().FetchGroupFSList(groupUin, targetDirectory);

    public static Task GroupFSDelete(this BotContext context, long groupUin, string fileId)
        => context.EventContext.GetLogic<OperationLogic>().GroupFSDelete(groupUin, fileId);

    public static Task GroupFSMove(this BotContext context, long groupUin, string fileId, string targetDirectory, string parentDirectory)
        => context.EventContext.GetLogic<OperationLogic>().GroupFSMove(groupUin, fileId, targetDirectory, parentDirectory);

    public static Task GroupFSCreateFolder(this BotContext context, long groupUin, string name, string parentFolderId = "/")
        => context.EventContext.GetLogic<OperationLogic>().GroupFSCreateFolder(groupUin, name, parentFolderId);

    public static Task GroupFSDeleteFolder(this BotContext context, long groupUin, string folderId)
        => context.EventContext.GetLogic<OperationLogic>().GroupFSDeleteFolder(groupUin, folderId);

    public static Task GroupFSRenameFolder(this BotContext context, long groupUin, string folderId, string newFolderName)
        => context.EventContext.GetLogic<OperationLogic>().GroupFSRenameFolder(groupUin, folderId, newFolderName);

    public static Task SendFriendNudge(this BotContext context, long peerUin, long? targetUin = null)
        => context.EventContext.GetLogic<OperationLogic>().SendNudge(false, peerUin, targetUin ?? context.BotUin);

    public static Task SendGroupNudge(this BotContext context, long peerUin, long targetUin)
        => context.EventContext.GetLogic<OperationLogic>().SendNudge(true, peerUin, targetUin);

    public static Task GroupSetSpecialTitle(this BotContext context, long groupUin, long targetUin, string title)
        => context.EventContext.GetLogic<OperationLogic>().GroupSetSpecialTitle(groupUin, targetUin, title);

    public static Task GroupMemberRename(this BotContext context, long groupUin, long targetUin, string name)
        => context.EventContext.GetLogic<OperationLogic>().GroupMemberRename(groupUin, targetUin, name);

    public static Task<bool> KickGroupMember(this BotContext context, long groupUin, long targetUin, bool rejectAddRequest, string reason = "")
        => context.EventContext.GetLogic<OperationLogic>().KickGroupMember(groupUin, targetUin, rejectAddRequest, reason);

    public static Task GroupRename(this BotContext context, long groupUin, string name)
        => context.EventContext.GetLogic<OperationLogic>().GroupRename(groupUin, name);

    public static Task GroupQuit(this BotContext context, long groupUin)
        => context.EventContext.GetLogic<OperationLogic>().GroupQuit(groupUin);
}
