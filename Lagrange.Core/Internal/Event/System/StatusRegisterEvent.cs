#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.System;

internal class StatusRegisterEvent : ProtocolEvent
{
    public string Message { get; set; }
    
    private StatusRegisterEvent() : base(true) { }

    private StatusRegisterEvent(string result) : base(0) => Message = result;
    
    public static StatusRegisterEvent Create() => new();

    public static StatusRegisterEvent Result(string result) => new(result);
}