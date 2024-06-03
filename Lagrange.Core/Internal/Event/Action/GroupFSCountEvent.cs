namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSCountEvent : GroupFSViewEvent
{
    public uint FileCount { get; set; }
    
    public uint LimitCount { get; set; }
    
    public bool IsFull { get; set; }
    
    private GroupFSCountEvent(uint groupUin) : base(groupUin) { }

    private GroupFSCountEvent(int resultCode, uint fileCount, uint limitCount, bool isFull) : base(resultCode)
    {
        FileCount = fileCount;
        LimitCount = limitCount;
        IsFull = isFull;
    }
    
    public static GroupFSCountEvent Create(uint groupUin) => new(groupUin);

    public static GroupFSCountEvent Result(int resultCode, uint fileCount, uint limitCount, bool isFull) 
        => new(resultCode, fileCount, limitCount, isFull);
}