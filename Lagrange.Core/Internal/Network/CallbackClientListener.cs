namespace Lagrange.Core.Internal.Network;

internal sealed class CallbackClientListener : ClientListener
{
    public override uint HeaderSize => _listener.HeaderSize;

    private readonly IClientListener _listener;

    public CallbackClientListener(IClientListener listener) => _listener = listener;

    public override uint GetPacketLength(ReadOnlySpan<byte> header) => _listener.GetPacketLength(header);

    public override void OnDisconnect() => _listener.OnDisconnect();

    public override void OnRecvPacket(ReadOnlySpan<byte> packet) => _listener.OnRecvPacket(packet);

    public override void OnSocketError(Exception e, ReadOnlyMemory<byte> data = default) => _listener.OnSocketError(e, data);
}