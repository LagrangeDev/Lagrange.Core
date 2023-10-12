namespace Lagrange.OneBot.Core.Message;

[AttributeUsage(AttributeTargets.Class)]
public class SegmentSubscriberAttribute(Type entity, string type) : Attribute
{
    public Type Entity { get; } = entity;

    public string Type { get; } = type;
}