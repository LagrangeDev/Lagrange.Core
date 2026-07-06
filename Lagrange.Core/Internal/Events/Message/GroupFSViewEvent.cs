using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Events.Message;

internal abstract class GroupFSViewEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class GroupFSListEventReq(long groupUin, string targetDirectory, uint startIndex, uint fileCount)
    : GroupFSViewEventReq(groupUin)
{
    public string TargetDirectory { get; } = targetDirectory;

    public uint StartIndex { get; } = startIndex;

    public uint FileCount { get; } = fileCount;
}

internal class GroupFSCountEventReq(long groupUin) : GroupFSViewEventReq(groupUin);

internal class GroupFSSpaceEventReq(long groupUin) : GroupFSViewEventReq(groupUin);

internal abstract class GroupFSViewEventResp(int resultCode, string? retMsg) : ProtocolEvent
{
    public int ResultCode { get; } = resultCode;

    public string? RetMsg { get; } = retMsg;
}

internal class GroupFSListEventResp(int resultCode, string? retMsg, List<IBotFSEntry> fileEntries, bool isEnd)
    : GroupFSViewEventResp(resultCode, retMsg)
{
    public List<IBotFSEntry> FileEntries { get; } = fileEntries;

    public bool IsEnd { get; } = isEnd;
}

internal class GroupFSCountEventResp(int resultCode, string? retMsg, uint fileCount, uint limitCount, bool isFull)
    : GroupFSViewEventResp(resultCode, retMsg)
{
    public uint FileCount { get; } = fileCount;

    public uint LimitCount { get; } = limitCount;

    public bool IsFull { get; } = isFull;
}

internal class GroupFSSpaceEventResp(int resultCode, string? retMsg, ulong totalSpace, ulong usedSpace)
    : GroupFSViewEventResp(resultCode, retMsg)
{
    public ulong TotalSpace { get; } = totalSpace;

    public ulong UsedSpace { get; } = usedSpace;
}
