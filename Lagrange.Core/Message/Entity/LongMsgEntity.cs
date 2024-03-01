using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(GeneralFlags))]
public class LongMsgEntity : IMessageEntity
{
    public string ResId { get; internal set; }
    
    public MessageChain Chain { get; internal set; }

    internal LongMsgEntity()
    {
        ResId = string.Empty;
        Chain = MessageBuilder.Group(0).Build();
    }

    public LongMsgEntity(string resId)
    {
        ResId = resId;
        Chain = MessageBuilder.Group(0).Build();
    }
    
    public LongMsgEntity(MessageChain chain)
    {
        ResId = string.Empty;
        Chain = chain;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement() => new Elem[]
    {
        new()
        {
            GeneralFlags = new GeneralFlags
            {
                LongTextResId = ResId,
                LongTextFlag = 1
            }
        }
    };

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        return elem.GeneralFlags is { LongTextResId: not null, LongTextFlag: 1 } msg
            ? new LongMsgEntity(msg.LongTextResId)
            : null;
    }

    public string ToPreviewString() => $"[{nameof(LongMsgEntity)}] {Chain.ToPreviewString()}";
}