namespace Lagrange.Core.Core.Network.Tcp;

internal interface IClientListener
{
    uint HeaderSize { get; }

    /// <summary>
    /// Dissect a stream
    /// </summary>
    /// <returns></returns>
    public uint GetPacketLength(ReadOnlySpan<byte> header);

    /// <summary>
    /// On handle a packet
    /// </summary>
    public void OnRecvPacket(ReadOnlySpan<byte> packet);

    /// <summary>
    /// On client disconnect
    /// </summary>
    public void OnDisconnect();

    /// <summary>
    /// On socket error
    /// </summary>
    public void OnSocketError(Exception e);
}