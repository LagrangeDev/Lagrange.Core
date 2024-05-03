namespace Lagrange.Core.Internal.Event.System;

internal class FetchRKeyEvent : ProtocolEvent
{
    public List<string> RKeys { get; } = new();

    private FetchRKeyEvent() : base(true)
    {
    }

    private FetchRKeyEvent(int resultCode, List<string> rKeys) : base(resultCode)
    {
        RKeys = rKeys;
    }
    
    public static FetchRKeyEvent Create() => new();
    
    public static FetchRKeyEvent Result(int resultCode, List<string> rKeys) => new(resultCode, rKeys);
}