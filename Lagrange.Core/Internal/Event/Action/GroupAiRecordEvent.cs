using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal class GroupAiRecordEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    public string Character { get; set; }
    public string Text { get; set; }

    public RecordEntity Record { get; set; }
    public string ErrorMessage { get; set; }

    private GroupAiRecordEvent(uint groupUin, string character, string text) : base(0)
    {
        GroupUin = groupUin;
        Character = character;
        Text = text;
        Record = new RecordEntity();
        ErrorMessage = string.Empty;
    }

    private GroupAiRecordEvent(int resultCode, RecordEntity recordEntity) : base(resultCode)
    {
        Character = string.Empty;
        Text = string.Empty;
        Record = recordEntity;
        ErrorMessage = string.Empty;
    }

    private GroupAiRecordEvent(int resultCode, string errMsg) : base(resultCode)
    {
        Character = string.Empty;
        Text = string.Empty;
        ErrorMessage = errMsg;
        Record = new();
    }

    public static GroupAiRecordEvent Create(uint groupUin, string character, string text) =>
        new(groupUin, character, text);

    public static GroupAiRecordEvent Result(int resultCode, RecordEntity recordEntity) => new(resultCode, recordEntity);

    public static GroupAiRecordEvent Result(int resultCode, string errMessage) => new(resultCode, errMessage);
}