using System.Text.Json.Serialization;
using Lagrange.Core.Message;

namespace Lagrange.OneBot.Core.Message;

public interface ISegment
{
    public IMessageEntity ToEntity();

    public void Build(MessageBuilder builder, ISegment segment);
    
    public ISegment FromEntity(IMessageEntity entity);
}