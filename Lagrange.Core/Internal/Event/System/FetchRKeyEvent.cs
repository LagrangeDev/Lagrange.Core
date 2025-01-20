using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchRKeyEvent : ProtocolEvent
{
    public List<RKeyInfo> RKeys { get; } = new();

    private FetchRKeyEvent() : base(true) { }

    private FetchRKeyEvent(int resultCode, List<RKeyInfo> rKeys) : base(resultCode)
    {
        RKeys = rKeys;
    }

    public static FetchRKeyEvent Create() => new();

    public static FetchRKeyEvent Result(int resultCode, List<RKeyInfo> rKeys) => new(resultCode, rKeys);
}