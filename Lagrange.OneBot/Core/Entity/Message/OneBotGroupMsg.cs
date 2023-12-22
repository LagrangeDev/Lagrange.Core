using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupMsg(uint selfId, uint groupUin, List<OneBotSegment> message, BotGroupMember member, uint messageId) : OneBotEntityBase(selfId, "message")
{
    [JsonPropertyName("message_type")] public string MessageType { get; set; } = "group";

    [JsonPropertyName("sub_type")] public string SubType { get; set; } = "normal";

    [JsonPropertyName("message_id")] public uint MessageId { get; set; } = messageId;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupUin;
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = member.Uin;
    
    [JsonPropertyName("anonymous")] public object? Anonymous { get; set; } = null;

    [JsonPropertyName("message")] public List<OneBotSegment> Message { get; set; } = message;

    [JsonPropertyName("raw_message")] public string RawMessage { get; set; } = string.Empty;

    [JsonPropertyName("font")] public int Font { get; set; } = 0;

    [JsonPropertyName("sender")] public OneBotGroupSender GroupSender { get; set; } = new(member.Uin, member.MemberName, member.MemberCard, (int)member.GroupLevel, member.Permission);
}