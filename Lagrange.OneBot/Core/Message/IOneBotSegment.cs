using Lagrange.Core.Message;

namespace Lagrange.OneBot.Core.Message;

public interface IOneBotSegment<TEntity> : ISegment where TEntity : IMessageEntity
{
    internal string Type { get; }
    
    public TEntity ToEntity();
    
    public ISegment FromMessageEntity(TEntity entity);
}