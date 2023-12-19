namespace Lagrange.Core.Internal.Event.System;

internal class CorrectTimeEvent : ProtocolEvent
{
    private CorrectTimeEvent() : base(false) { }

    private CorrectTimeEvent(int resultCode) : base(resultCode) { }
    
    public static CorrectTimeEvent Create() => new();
    
    public static CorrectTimeEvent Result() => new(0);
}