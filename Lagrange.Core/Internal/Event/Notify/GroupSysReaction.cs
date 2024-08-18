namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysReactionEvent : ProtocolEvent
{
    public uint TargetGroupUin { get; }

    public uint TargetSequence { get; }

    public string OperatorUid { get; }

    public bool IsAdd { get; }

    public string Code { get; }

    private GroupSysReactionEvent(uint targetGroupUin, uint targetSequence, string operatorUid, bool isAdd, string code) : base(0)
    {
        TargetGroupUin = targetGroupUin;
        TargetSequence = targetSequence;
        OperatorUid = operatorUid;
        IsAdd = isAdd;
        Code = code;
    }

    public static GroupSysReactionEvent Result(uint groupUin, uint targetSequence, string operatorUid, bool isAdd, string code)
        => new(groupUin, targetSequence, operatorUid, isAdd, code);
}