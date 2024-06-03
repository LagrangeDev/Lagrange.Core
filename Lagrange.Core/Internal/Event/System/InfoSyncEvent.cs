namespace Lagrange.Core.Internal.Event.System;

internal class InfoSyncEvent : ProtocolEvent
{
    public uint Random { get; set; }

    private InfoSyncEvent() : base(true)
    {
        Random = (uint) new Random().Next();
    }
    
    private InfoSyncEvent(int resultCode) : base(resultCode) { }
    
    public static InfoSyncEvent Create() => new();
    
    public static InfoSyncEvent Result(int resultCode) => new(resultCode);
}