using Lagrange.Core.Core.Packets.Message.Component;
using Lagrange.Core.Core.Packets.Message.Element;

namespace Lagrange.Core.Message;

public interface IMessageEntity
{
    /// <summary>
    /// Pack the message into the Protobuf, and this would be packed into the list of elements.
    /// <para>The Final Packet would be here <see cref="RichText"/></para>
    /// </summary>
    /// <returns></returns>
    internal IEnumerable<Elem> PackElement(); // abstract method

    internal object? PackMessageContent() => null; // virtual method

    internal IMessageEntity? UnpackElement(Elem elem); // abstract method
    
    internal IMessageEntity? UnpackMessageContent(ReadOnlySpan<byte> content) => null; // virtual method

    internal void SetSelfUid(string selfUid) { } // virtual method

    public string ToPreviewString(); // abstract method
}