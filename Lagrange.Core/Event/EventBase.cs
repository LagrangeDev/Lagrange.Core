namespace Lagrange.Core.Event;

/// <summary>
/// Event that exposed to user
/// </summary>
public abstract class EventBase : EventArgs
{
    public DateTime EventTime { get; }

    public string EventMessage { get; protected set; }

    internal EventBase()
    {
        EventTime = DateTime.Now;
        EventMessage = "[Empty Event Message]";
    }
    
    public override string ToString()
    {
        return $"[{EventTime:HH:mm:ss}] {EventMessage}";
    }
}