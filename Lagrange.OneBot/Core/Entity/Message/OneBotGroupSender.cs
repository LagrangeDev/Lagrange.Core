using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupSender
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("nickname")] public string Nickname { get; set; }
    
    [JsonPropertyName("card")] public string Card { get; set; }
    
    [JsonPropertyName("sex")] public string Sex { get; set; }
    
    [JsonPropertyName("age")] public uint Age { get; set; }
    
    [JsonPropertyName("area")] public string Area { get; set; }
    
    [JsonPropertyName("level")] public string Level { get; set; }
    
    [JsonPropertyName("role")] public string Role { get; set; }
    
    [JsonPropertyName("title")] public string Title { get; set; }
    
    public OneBotGroupSender(uint userId, string nickname, string card, int level, GroupMemberPermission permission)
    {
        UserId = userId;
        Nickname = nickname;
        Card = card;
        Sex = "unknown";
        Age = 0;
        Area = string.Empty;
        Level = level.ToString();
        Role = permission switch
        {
            GroupMemberPermission.Owner => "owner",
            GroupMemberPermission.Admin => "admin",
            GroupMemberPermission.Member => "member",
            _ => "unknown"
        };
        Title = string.Empty;
    }
}