using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal class GroupClockInEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    public BotGroupClockInResult? ResultInfo { get; set; }

    private GroupClockInEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
        ResultInfo = null;
    }

    private GroupClockInEvent(int resultCode, BotGroupClockInResult result) : base(resultCode)
    {
        ResultInfo = result;
    }

    public static GroupClockInEvent Create(uint groupUin) => new(groupUin);

    public static GroupClockInEvent Result(int resultCode, BotGroupClockInResult result) => new(resultCode, result);
}
