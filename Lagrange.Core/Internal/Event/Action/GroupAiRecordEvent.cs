using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Action;

internal class GroupAiRecordEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    public string Character { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public uint ChatType { get; set; }
    public uint ChatId { get; set; }

    public MsgInfo? RecordInfo { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    private GroupAiRecordEvent(uint groupUin, string character, string text, uint chatType, uint chatId) : base(0)
    {
        GroupUin = groupUin;
        Character = character;
        Text = text;
        ChatType = chatType;
        ChatId = chatId;
    }

    private GroupAiRecordEvent(int resultCode, MsgInfo? msgInfo) : base(resultCode)
    {
        RecordInfo = msgInfo;
    }

    private GroupAiRecordEvent(int resultCode, string errMsg) : base(resultCode)
    {
        ErrorMessage = errMsg;
    }

    public static GroupAiRecordEvent Create(uint groupUin, string character, string text, uint chatType, uint chatId) =>
        new(groupUin, character, text, chatType, chatId);

    public static GroupAiRecordEvent Result(int resultCode, MsgInfo? msgInfo) => new(resultCode, msgInfo);

    public static GroupAiRecordEvent Result(int resultCode, string errMessage) => new(resultCode, errMessage);
}