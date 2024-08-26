namespace Lagrange.Core.Internal.Event.Login;

internal class NewDeviceLoginEvent : ProtocolEvent
{
    public bool Success { get; set; }

    private NewDeviceLoginEvent() : base(true) { }

    private NewDeviceLoginEvent(int result) : base(result)
    {
        Success = result == 0;
    }

    public static NewDeviceLoginEvent Create() => new();

    public static NewDeviceLoginEvent Result(int result) => new(result);
}