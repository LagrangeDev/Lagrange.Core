using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotNode(uint userId, string nickName, List<OneBotSegment> content) : ISegment
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("nickname")] public string NickName { get; set; } = nickName;

    [JsonPropertyName("content")] public List<OneBotSegment> Content { get; set; } = content;
    
    public IMessageEntity ToEntity()
    {
        throw new NotImplementedException();
    }

    public void Build(MessageBuilder builder, ISegment segment)
    {
        throw new NotImplementedException();
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        throw new NotImplementedException();
    }
}