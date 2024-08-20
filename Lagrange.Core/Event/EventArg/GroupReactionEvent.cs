namespace Lagrange.Core.Event.EventArg;

public class GroupReactionEvent : EventBase
{
    public uint TargetGroupUin { get; }

    public uint TargetSequence { get; }

    public uint OperatorUin { get; }

    public bool IsAdd { get; }

    public string Code { get; }

    public uint Count { get; }

    public GroupReactionEvent(uint targetGroupUin, uint targetSequence, uint operatorUin, bool isAdd, string code, uint count)
    {
        TargetGroupUin = targetGroupUin;
        TargetSequence = targetSequence;
        OperatorUin = operatorUin;
        IsAdd = isAdd;
        Code = code;
        Count = count;

        EventMessage = $"{nameof(GroupReactionEvent)}:  TargetGroupUin: {TargetGroupUin} | TargetSequence: {TargetSequence} | OperatorUin: {OperatorUin} | IsAdd: {IsAdd} | Code: {Code} | Count: {Count}";
    }
}