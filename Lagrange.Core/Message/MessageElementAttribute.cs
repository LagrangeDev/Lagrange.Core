namespace Lagrange.Core.Message;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class MessageElementAttribute : Attribute
{
    public Type Element { get; }

    public MessageElementAttribute(Type element) => Element = element;
}