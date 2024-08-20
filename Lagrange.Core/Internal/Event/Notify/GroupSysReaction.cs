namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysReactionEvent : ProtocolEvent
{
    public uint TargetGroupUin { get; }

    public uint TargetSequence { get; }

    public string OperatorUid { get; }

    public bool IsAdd { get; }

    public string Code { get; }

    public uint Count { get; }

    private GroupSysReactionEvent(uint targetGroupUin, uint targetSequence, string operatorUid, bool isAdd, string code,uint count) : base(0)
    {
        TargetGroupUin = targetGroupUin;
        TargetSequence = targetSequence;
        OperatorUid = operatorUid;
        IsAdd = isAdd;
        Code = code;
        Count = count;
    }

    public static GroupSysReactionEvent Result(uint groupUin, uint targetSequence, string operatorUid, bool isAdd, string code, uint count)
        => new(groupUin, targetSequence, operatorUid, isAdd, code, count);
}