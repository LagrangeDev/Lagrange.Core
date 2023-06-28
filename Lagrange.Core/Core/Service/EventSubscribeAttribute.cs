namespace Lagrange.Core.Core.Service;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class EventSubscribeAttribute : Attribute
{
    public Type EventType { get; }
    
    public EventSubscribeAttribute(Type eventType)
    {
        EventType = eventType;
    }
}