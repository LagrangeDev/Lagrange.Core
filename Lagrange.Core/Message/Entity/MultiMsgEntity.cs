using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Element.Implementation;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(GeneralFlags))]
public class MultiMsgEntity : IMessageEntity
{
    internal string? ResId { get; }
    
    public List<MessageChain> Chains { get; }
    
    internal MultiMsgEntity() => Chains = new List<MessageChain>();

    internal MultiMsgEntity(string resId)
    {
        ResId = resId;
        Chains = new List<MessageChain>();
    }
    
    internal MultiMsgEntity(List<MessageChain> chains)
    {
        Chains = chains;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        throw new NotImplementedException();
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem) => 
            elem.GeneralFlags is { LongTextResId: not null } ? new MultiMsgEntity(elem.GeneralFlags.LongTextResId) : null;

    public string ToPreviewString() => $"[MultiMsgEntity] {Chains.Count} chains";
}