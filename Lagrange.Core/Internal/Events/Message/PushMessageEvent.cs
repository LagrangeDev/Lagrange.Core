using Lagrange.Core.Events;
using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Internal.Events.Message;

internal class PushMessageEvent(MsgPush msg, ReadOnlyMemory<byte> raw) : ProtocolEvent
{
    public MsgPush MsgPush { get; } = msg;

    internal ReadOnlyMemory<byte> Raw { get; } = raw;
}