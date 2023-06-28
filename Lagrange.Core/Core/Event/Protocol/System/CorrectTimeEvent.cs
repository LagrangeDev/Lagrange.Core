namespace Lagrange.Core.Core.Event.Protocol.System;

internal class CorrectTimeEvent : ProtocolEvent
{
    protected CorrectTimeEvent() : base(false)
    {
    }

    protected CorrectTimeEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static CorrectTimeEvent Create() => new();
    
    public static CorrectTimeEvent Result() => new(0);
}