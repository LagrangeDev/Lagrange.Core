namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysSignInEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public string Action { get; }

    public string RankImage { get; }

    private GroupSysSignInEvent(uint groupUin, uint operatorUin, string action, string rankImage) : base(0)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        Action = action;
        RankImage = rankImage;
    }

    public static GroupSysSignInEvent Result(uint groupUin, uint operatorUin, string action, string rankImage) => new(groupUin, operatorUin, action, rankImage);
}