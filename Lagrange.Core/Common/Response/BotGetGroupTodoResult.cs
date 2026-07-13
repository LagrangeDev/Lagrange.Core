namespace Lagrange.Core.Common.Response;

[Serializable]
public class BotGetGroupTodoResult(ulong groupUin, ulong sequence, string preview)
{
    public ulong GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;

    public string Preview { get; } = preview;
}
