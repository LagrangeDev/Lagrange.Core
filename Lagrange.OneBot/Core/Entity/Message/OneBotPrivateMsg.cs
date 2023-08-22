using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotPrivateMsg : OneBotEntityBase
{
    [JsonPropertyName("message_type")] public string MessageType { get; set; }
    
    [JsonPropertyName("sub_type")] public string SubType { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("message")] public List<ISegment> Message { get; set; }
    
    [JsonPropertyName("raw_message")] public string RawMessage { get; set; }
    
    [JsonPropertyName("font")] public int Font { get; set; }
    
    [JsonPropertyName("sender")] public OneBotSender GroupSender { get; set; }
    
    public OneBotPrivateMsg(uint selfId) : base(selfId, "message")
    {
        MessageType = "private";
        SubType = "friend";
        Message = new List<ISegment>();
        RawMessage = string.Empty;
        Font = 0;
        GroupSender = new OneBotSender();
    }
}