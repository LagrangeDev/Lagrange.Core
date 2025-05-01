using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal partial class SetGroupRequestEvent : ProtocolEvent
{
    public ulong Sequence { get; set; }

    public uint GroupUin { get; set; }

    public GroupRequestOperate Operate { get; set; }

    public uint Type { get; }

    public string Reason { get; set; } = string.Empty;

    private SetGroupRequestEvent(GroupRequestOperate operate, uint groupUin, ulong sequence, uint type, string? reason) : base(true)
    {
        Operate = operate;
        GroupUin = groupUin;
        Sequence = sequence;
        Type = type;
        Reason = reason ?? "";
    }

    private SetGroupRequestEvent(int resultCode) : base(resultCode) { }

    public static SetGroupRequestEvent Create(GroupRequestOperate operate, uint groupUin, ulong sequence, uint type, string? reason)
        => new(operate, groupUin, sequence, type, reason);

    public static SetGroupRequestEvent Result(int resultCode) => new(resultCode);
}