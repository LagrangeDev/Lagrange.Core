namespace Lagrange.Core.Internal.Event.System;

// ReSharper disable once InconsistentNaming

internal class KickNTEvent : ProtocolEvent
{
    public string Tag { get; set; }
    
    public string Message { get; set; }
    
    private KickNTEvent(string tag, string message) : base(0)
    {
        Tag = tag;
        Message = message;
    }
    
    public static KickNTEvent Create(string tag, string message) => new(tag, message);
}