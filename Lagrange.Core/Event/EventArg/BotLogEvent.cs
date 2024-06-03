namespace Lagrange.Core.Event.EventArg;

public enum LogLevel
{
    Debug,
    Verbose,
    Information,
    Warning,
    Exception,
    Fatal
}

public class BotLogEvent : EventBase
{
    private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    
    public string Tag { get; }

    public LogLevel Level { get; }

    internal BotLogEvent(string tag, LogLevel level, string content)
    {
        Tag = tag;
        Level = level;
        EventMessage = content;
    }

    public override string ToString() => 
        $"[{EventTime.ToString(DateFormat)}] [{Tag}] [{Level.ToString().ToUpper()}]: {EventMessage}";
}