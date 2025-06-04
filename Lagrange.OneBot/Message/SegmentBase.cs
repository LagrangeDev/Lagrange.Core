using Lagrange.Core.Message;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Message;

public abstract class SegmentBase
{
    public RealmHelper? Realm { protected get; set; }
    
    public MessageService? MessageService { protected get; set; } // just for reply segment inflat
    
    public abstract void Build(MessageBuilder builder, SegmentBase segment);
    
    public abstract SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity);
}