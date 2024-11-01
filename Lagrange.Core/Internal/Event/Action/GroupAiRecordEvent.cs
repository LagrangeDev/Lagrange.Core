using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal class GroupAiRecordEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    public string Character { get; set; }
    public string Text { get; set; }

    public MsgInfo? RecordInfo { get; set; }
    public string ErrorMessage { get; set; }

    private GroupAiRecordEvent(uint groupUin, string character, string text) : base(0)
    {
        GroupUin = groupUin;
        Character = character;
        Text = text;
        ErrorMessage = string.Empty;
    }

    private GroupAiRecordEvent(int resultCode, MsgInfo? msgInfo) : base(resultCode)
    {
        Character = string.Empty;
        Text = string.Empty;
        ErrorMessage = string.Empty;
        RecordInfo = msgInfo;
    }

    private GroupAiRecordEvent(int resultCode, string errMsg) : base(resultCode)
    {
        Character = string.Empty;
        Text = string.Empty;
        ErrorMessage = errMsg;
    }

    public static GroupAiRecordEvent Create(uint groupUin, string character, string text) =>
        new(groupUin, character, text);

    public static GroupAiRecordEvent Result(int resultCode, MsgInfo? msgInfo) => new(resultCode, msgInfo);

    public static GroupAiRecordEvent Result(int resultCode, string errMessage) => new(resultCode, errMessage);
}