using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupFSListEvent : GroupFSViewEvent
{
    public string TargetDirectory { get; set; }
    
    public uint StartIndex { get; set; }
    
    public List<IBotFSEntry> FileEntries { get; set; }

    private GroupFSListEvent(uint groupUin, string targetDirectory, uint startIndex) : base(groupUin)
    {
        TargetDirectory = targetDirectory;
        StartIndex = startIndex;
    }

    private GroupFSListEvent(int resultCode, List<IBotFSEntry> fileEntries) : base(resultCode)
    {
        FileEntries = fileEntries;
    }

    public static GroupFSListEvent Create(uint groupUin, string targetDirectory, uint startIndex) 
        => new(groupUin, targetDirectory, startIndex);

    public static GroupFSListEvent Result(int resultCode, List<IBotFSEntry> fileEntries) 
        => new(resultCode, fileEntries);
}