namespace Lagrange.Core.Event;

/// <summary>
/// Event that exposed to user
/// </summary>
public abstract class EventBase : EventArgs
{
    public DateTimeOffset EventTime { get; }

    public string EventMessage { get; protected set; }

    internal EventBase()
    {
        EventTime = DateTimeOffset.Now;
        EventMessage = "[Empty Event Message]";
    }
    
    public override string ToString()
    {
        return $"[{EventTime:HH:mm:ss}] {EventMessage}";
    }
}