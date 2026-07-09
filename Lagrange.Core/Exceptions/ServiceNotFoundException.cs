namespace Lagrange.Core.Exceptions;

/// <summary>
/// Thrown when no protocol service is registered for a command or event type.
/// </summary>
public class ServiceNotFoundException : LagrangeException
{
    public ServiceNotFoundException(string command) : base($"Protocol service not found for command: {command}")
    {
        Command = command;
    }

    public ServiceNotFoundException(Type eventType) : base($"Protocol service not found for event type: {eventType}")
    {
        EventType = eventType;
    }

    public string? Command { get; }

    public Type? EventType { get; }
}
