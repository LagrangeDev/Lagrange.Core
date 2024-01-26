using Lagrange.Core.Internal.Packets.Message.Element;

namespace Lagrange.Core.Message.Entity;

public class RecordEntity : IMessageEntity
{
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        throw new NotImplementedException();
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        throw new NotImplementedException();
    }

    public string ToPreviewString()
    {
        throw new NotImplementedException();
    }
}