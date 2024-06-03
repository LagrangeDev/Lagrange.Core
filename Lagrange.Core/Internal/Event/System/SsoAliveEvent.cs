namespace Lagrange.Core.Internal.Event.System;

internal class SsoAliveEvent : ProtocolEvent
{
    private SsoAliveEvent() : base(true) { }

    private SsoAliveEvent(int resultCode) : base(resultCode) { }
    
    public static SsoAliveEvent Create() => new();
    
    public static SsoAliveEvent Result() => new(0);
}