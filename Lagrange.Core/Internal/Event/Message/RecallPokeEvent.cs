
namespace Lagrange.Core.Internal.Event.Message;

internal class RecallPokeEvent : ProtocolEvent
{
    public bool IsGroup { get; init; }
    public ulong Uin { get; init; }
    public ulong MessageSequence { get; init; }
    public ulong MessageTime { get; init; }
    public ulong TipsSeqId { get; internal set; }

    public string Message { get; set; }

    private RecallPokeEvent(bool isGroup, ulong uin, ulong messageSequence, ulong messageTime, ulong tipsSeqId) : base(true)
    {
        IsGroup = isGroup;
        Uin = uin;
        MessageSequence = messageSequence;
        MessageTime = messageTime;
        TipsSeqId = tipsSeqId;

        Message = null!;
    }

    private RecallPokeEvent(int retcode, string message) : base(retcode)
    {
        Message = message;
    }

    public static RecallPokeEvent Create(bool isGroup, ulong uin, ulong messageSequence, ulong messageTime, ulong tipsSeqId)
        => new(isGroup, uin, messageSequence, messageTime, tipsSeqId);

    public static RecallPokeEvent Result(int retcode, string message)
        => new(retcode, message);
}