namespace Lagrange.Core.Event.EventArg;

public class BotNewDeviceVerifyEvent : EventBase
{
    public string Url { get; }
    
    public byte[] QrCode { get; }
    
    public BotNewDeviceVerifyEvent(string url, byte[] qrCode) 
    {
        Url = url;
        EventMessage = $"[{nameof(BotNewDeviceVerifyEvent)}]: Url: {url}";
        QrCode = qrCode;
    }
}