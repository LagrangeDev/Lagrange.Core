using Lagrange.Core.Message;
using LiteDB;

namespace Lagrange.OneBot.Core.Message;

public abstract class SegmentBase
{
    public LiteDatabase? Database { protected get; set; }
    
    public abstract void Build(MessageBuilder builder, SegmentBase segment);
    
    public abstract SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity);
}