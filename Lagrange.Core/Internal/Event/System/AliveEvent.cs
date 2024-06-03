namespace Lagrange.Core.Internal.Event.System;

internal class AliveEvent : ProtocolEvent
{
    private AliveEvent() : base(false) { }

    private AliveEvent(int resultCode) : base(resultCode) { }
    
    public static AliveEvent Create() => new();
}