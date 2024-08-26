using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupFSListEvent : GroupFSViewEvent
{
    public string TargetDirectory { get; set; }
    
    public uint StartIndex { get; set; }
    
    public uint FileCount { get; set; }
    
    public bool IsEnd { get; set; }
    
    public List<IBotFSEntry> FileEntries { get; set; }

    private GroupFSListEvent(uint groupUin, string targetDirectory, uint startIndex, uint fileCount) : base(groupUin)
    {
        TargetDirectory = targetDirectory;
        StartIndex = startIndex;
        FileCount = fileCount;
        IsEnd = false;
    }

    private GroupFSListEvent(int resultCode, List<IBotFSEntry> fileEntries, bool isEnd) : base(resultCode)
    {
        FileEntries = fileEntries;
        IsEnd = isEnd;
    }

    public static GroupFSListEvent Create(uint groupUin, string targetDirectory, uint startIndex, uint fileCount) 
        => new(groupUin, targetDirectory, startIndex, fileCount);

    public static GroupFSListEvent Result(int resultCode, List<IBotFSEntry> fileEntries, bool isEnd) 
        => new(resultCode, fileEntries, isEnd);
}