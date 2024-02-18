using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.OneBot.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotNode(uint userId, string nickName, List<OneBotSegment> content) : SegmentBase
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("nickname")] public string NickName { get; set; } = nickName;

    [JsonPropertyName("content")] public List<OneBotSegment> Content { get; set; } = content;
    
    public IMessageEntity ToEntity() => throw new NotImplementedException();

    public override void Build(MessageBuilder builder, SegmentBase segment) => throw new NotImplementedException();

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity) => throw new NotImplementedException();
}