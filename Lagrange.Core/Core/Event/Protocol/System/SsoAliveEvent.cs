namespace Lagrange.Core.Core.Event.Protocol.System;

internal class SsoAliveEvent : ProtocolEvent
{
    private SsoAliveEvent() : base(true) { }

    private SsoAliveEvent(int resultCode) : base(resultCode) { }
    
    public static SsoAliveEvent Create() => new();
    
    public static SsoAliveEvent Result() => new(0);
}