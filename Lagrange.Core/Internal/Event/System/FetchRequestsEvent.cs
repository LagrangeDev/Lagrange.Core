namespace Lagrange.Core.Internal.Event.System;

internal class FetchRequestsEvent : ProtocolEvent
{
    private FetchRequestsEvent() : base(true) { }

    private FetchRequestsEvent(int resultCode) : base(resultCode) { }

    public static FetchRequestsEvent Create() => new();
}