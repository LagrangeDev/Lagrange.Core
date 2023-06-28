namespace Lagrange.Core.Core.Event.Protocol.System;

internal class AliveEvent : ProtocolEvent
{
    protected AliveEvent() : base(false) { }

    protected AliveEvent(int resultCode) : base(resultCode) { }
    
    public static AliveEvent Create() => new();
    
    public static AliveEvent Result() => new(0);
}