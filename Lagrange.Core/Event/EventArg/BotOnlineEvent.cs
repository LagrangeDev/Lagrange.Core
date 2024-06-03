namespace Lagrange.Core.Event.EventArg;

public class BotOnlineEvent : EventBase
{
    public OnlineReason Reason { get; }
    
    public BotOnlineEvent(OnlineReason reason)
    {
        Reason = reason;
        EventMessage = $"[{nameof(BotOnlineEvent)}]: {Reason}";
    }
    
    public enum OnlineReason
    {
        Login,
        Reconnect
    }
}