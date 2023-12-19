namespace Lagrange.Core.Event.EventArg;

public class BotCaptchaEvent : EventBase
{
    public string Url { get; }
    
    public BotCaptchaEvent(string url) 
    {
        Url = url;
        EventMessage = $"[{nameof(BotCaptchaEvent)}]: Url: {url}";
    }
}