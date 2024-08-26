namespace Lagrange.Core.Event.EventArg;

public class BotOfflineEvent : EventBase
{
    public string Tag { get; }

    public string Message { get; }

    public BotOfflineEvent(string tag, string message)
    {
        Tag = tag;
        Message = message;
    }
}