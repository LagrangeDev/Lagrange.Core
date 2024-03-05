namespace Lagrange.Core.Event.EventArg;

public class BotNewDeviceVerifyEvent : EventBase
{
    public string PhoneNumber { get; }
    
    public BotNewDeviceVerifyEvent(string phoneNumber) 
    {
        PhoneNumber = phoneNumber;
        EventMessage = $"[{nameof(BotNewDeviceVerifyEvent)}]: PhoneNumber: {phoneNumber}";
    }
}