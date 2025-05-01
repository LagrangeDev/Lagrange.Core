using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal class SetGroupFilteredRequestEvent : ProtocolEvent
{
    public ulong Sequence { get; set; }

    public uint GroupUin { get; set; }
    
    public GroupRequestOperate Operate { get; set; }
    
    public uint Type { get; }
    
    public string Reason { get; set; } = string.Empty;

    private SetGroupFilteredRequestEvent(GroupRequestOperate operate, uint groupUin, ulong sequence, uint type, string? reason) : base(true)
    {
        Operate = operate;
        GroupUin = groupUin;
        Sequence = sequence;
        Type = type;
        Reason = reason ?? "";
    }
    
    private SetGroupFilteredRequestEvent(int resultCode) : base(resultCode) { }

    public static SetGroupFilteredRequestEvent Create(GroupRequestOperate operate, uint groupUin, ulong sequence, uint type, string? reason) 
        => new(operate, groupUin, sequence, type, reason);
    
    public static SetGroupFilteredRequestEvent Result(int resultCode) => new(resultCode);
}