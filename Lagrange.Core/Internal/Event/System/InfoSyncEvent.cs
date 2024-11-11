#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.System;

internal class InfoSyncEvent : ProtocolEvent
{
    public string Message { get; set; }
    
    private InfoSyncEvent() : base(true) { }

    private InfoSyncEvent(string result) : base(0) => Message = result;
    
    public static InfoSyncEvent Create() => new();

    public static InfoSyncEvent Result(string result) => new(result);
}