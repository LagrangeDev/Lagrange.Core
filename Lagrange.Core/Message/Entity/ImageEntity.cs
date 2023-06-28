using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Element.Implementation;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(NotOnlineImage))]
public class ImageEntity : IMessageEntity
{
    public ImageEntity() { }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                CommonElem = new CommonElem
                {
                    
                }
            },
            new()
            {
                NotOnlineImage = new NotOnlineImage
                {
                    
                }
            }
        };
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        throw new NotImplementedException();
    }

    public string ToPreviewString()
    {
        throw new NotImplementedException();
    }
}