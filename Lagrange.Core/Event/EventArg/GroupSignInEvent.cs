namespace Lagrange.Core.Event.EventArg;

public class GroupSignInEvent : EventBase
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public string Action { get; }

    public string RankImage { get; }

    public GroupSignInEvent(uint groupUin, uint operatorUin, string action, string rankImage)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        Action = action;
        RankImage = rankImage;

        EventMessage = $"{nameof(GroupSignInEvent)}: {GroupUin} | {OperatorUin} {Action} | {rankImage}";
    }
}