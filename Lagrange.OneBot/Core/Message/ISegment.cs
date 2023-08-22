using Lagrange.Core.Message;

namespace Lagrange.OneBot.Core.Message;

public interface ISegment
{
    internal string Type { get; }
    
    public IMessageEntity ToEntity();
    
    public ISegment FromMessageEntity(IMessageEntity entity);
}