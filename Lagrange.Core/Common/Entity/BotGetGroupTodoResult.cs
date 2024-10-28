namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotGetGroupTodoResult
{
    public int Retcode { get; }

    public string? ResultMessage { get; }

    public uint GroupUin { get; }

    public uint Sequence { get; }

    public string Preview { get; }

    public BotGetGroupTodoResult(int retcode, string? resultMessage, uint groupUin, uint sequence, string preview)
    {
        Retcode = retcode;
        ResultMessage = resultMessage;
        GroupUin = groupUin;
        Sequence = sequence;
        Preview = preview;
    }
}
