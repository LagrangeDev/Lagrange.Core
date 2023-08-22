using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupMsg : OneBotEntityBase
{
    [JsonPropertyName("message_type")] public string MessageType { get; set; }
    
    [JsonPropertyName("sub_type")] public string SubType { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }
    
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("anonymous")] public object? Anonymous { get; set; }
    
    [JsonPropertyName("message")] public List<ISegment> Message { get; set; }
    
    [JsonPropertyName("raw_message")] public string RawMessage { get; set; }
    
    [JsonPropertyName("font")] public int Font { get; set; }
    
    [JsonPropertyName("sender")] public OneBotGroupSender GroupSender { get; set; }

    public OneBotGroupMsg(uint selfId, List<ISegment> message, BotGroupMember member) : base(selfId, "message")
    {
        MessageType = "group";
        SubType = "normal";
        Anonymous = null;
        Message = message;
        RawMessage = string.Empty;
        Font = 0;
        GroupSender = new OneBotGroupSender(member.Uin, member.MemberName, member.MemberCard, (int)member.GroupLevel, member.Permission);
    }
}