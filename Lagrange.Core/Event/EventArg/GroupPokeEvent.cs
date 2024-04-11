namespace Lagrange.Core.Event.EventArg;

public class GroupPokeEvent : EventBase
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public uint TargetUin { get; }

    public string Action { get; }

    public string Suffix { get; }

    public string ActionImage { get; }

    public GroupPokeEvent(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix, string actionImage)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImage = actionImage;

        EventMessage = $"{nameof(GroupPokeEvent)}: {GroupUin} | {OperatorUin} {Action} {TargetUin} {Suffix} | {ActionImage}";
    }
}