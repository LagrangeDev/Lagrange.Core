namespace Lagrange.Core.Internal.Event.Login;

internal class EasyLoginEvent : ProtocolEvent
{
    private EasyLoginEvent() : base(true) { }

    private EasyLoginEvent(int result) : base(result) { }
    
    public static EasyLoginEvent Create() => new();

    public static EasyLoginEvent Result(int result) => new(result);
}