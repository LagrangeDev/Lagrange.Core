using Lagrange.Core.Core.Packets.Message.Element;

namespace Lagrange.Core.Message.Entity;

/// <summary>
/// 没错就是你想的那个Packet
/// </summary>
public class PacketEntity : IMessageEntity
{
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        throw new NotImplementedException();
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