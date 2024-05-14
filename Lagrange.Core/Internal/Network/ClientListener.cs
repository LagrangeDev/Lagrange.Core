using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Network;

internal abstract partial class ClientListener : IClientListener
{
    /// <summary>
    /// Socket connected
    /// </summary>
    public bool Connected => Session?.Socket.Connected ?? false;

    public abstract uint HeaderSize { get; }

    protected SocketSession? Session;

    /// <summary>
    /// Construct a tcp client
    /// </summary>
    public ClientListener() { }

    private async Task<bool> InternalConnectAsync(SocketSession session, string host, int port)
    {
        try
        {
            await session.Socket.ConnectAsync(host, port);
            _ = ReceiveLoop(session);
            return true;
        }
        catch (Exception e)
        {
            RemoveSession(session);
            OnSocketError(e);
            return false;
        }
    }

    /// <summary>
    /// Connect to the server
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public Task<bool> Connect(string host, int port)
    {
        SocketSession? previousSession = Session, createdSession = null;
        if (previousSession != null || // The client has been connected
            Interlocked.CompareExchange(ref Session, createdSession = new SocketSession(), null) != null) // Another connect request before this request
        {
            createdSession?.Dispose();
            return Task.FromResult(false);
        }

        return InternalConnectAsync(createdSession, host, port); // Connect to server
    }

    /// <summary>
    /// Disconnect
    /// </summary>
    /// <returns></returns>
    public void Disconnect()
    {
        if (Session is { } session) RemoveSession(session);
    }

    /// <summary>
    /// Send data
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public async Task<bool> Send(ReadOnlyMemory<byte> buffer, int timeout = -1)
    {
        try
        {
            var session = Session; // Send the data
            if (session == null) return false;

            CancellationTokenSource? userCts;
            CancellationTokenSource? linkedCts;
            CancellationToken token;
            if (timeout == -1)
            {
                userCts = null;
                linkedCts = null;
                token = session.Token;
            }
            else
            {
                userCts = new CancellationTokenSource(timeout);
                linkedCts = CancellationTokenSource.CreateLinkedTokenSource(session.Token, userCts.Token);
                token = linkedCts.Token;
            }
            
            try
            {
                return await session.Socket.SendAsync(buffer, SocketFlags.None, token) == buffer.Length;
            }
            finally
            {
                userCts?.Dispose();
                linkedCts?.Dispose();
            }
        }
        catch (Exception e)
        {
            OnSocketError(e, buffer);
            return false;
        }
    }

    /// <summary>
    /// Receive the data
    /// </summary>
    private async Task ReceiveLoop(SocketSession session, CancellationToken token = default)
    {
        try
        {
            await Task.CompletedTask.ForceAsync();
            Socket socket = session.Socket;
            byte[] buffer = new byte[Math.Max(HeaderSize, 2048)];
            int headerSize = (int)HeaderSize;
            while (true)
            {
                await socket.ReceiveFullyAsync(buffer.AsMemory(0, headerSize), token);
                int packetLength = (int)GetPacketLength(buffer.AsSpan(0, headerSize));
                if (packetLength > 1024 * 1024 * 64) // limit to 64MiB
                {
                    throw new InvalidDataException($"PackageLength was too long({packetLength}).");
                }
                if (packetLength > buffer.Length)
                {
                    byte[] newBuffer = new byte[packetLength];
                    Unsafe.CopyBlock(ref newBuffer[0], ref buffer[0], (uint)headerSize);
                    buffer = newBuffer;
                }
                await socket.ReceiveFullyAsync(buffer.AsMemory(headerSize, packetLength - headerSize), token);
                
                try
                {
                    OnRecvPacket(buffer.AsSpan(0, packetLength));
                }
                catch (Exception e)
                {
                    OnSocketError(e, buffer.AsMemory(0, packetLength));
                    throw;
                }
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {

        }
        catch (SocketException e) when (e.SocketErrorCode == SocketError.OperationAborted)
        {

        }
        catch (Exception e)
        {
            OnSocketError(e);
        }
        finally
        {
            RemoveSession(session);
        }
    }

    private void RemoveSession(SocketSession session)
    {
        if (Interlocked.CompareExchange(ref Session, null, session) == session)
        {
            session.Dispose();
        }
    }

    public abstract uint GetPacketLength(ReadOnlySpan<byte> header);

    public abstract void OnRecvPacket(ReadOnlySpan<byte> packet);

    public abstract void OnDisconnect();

    public abstract void OnSocketError(Exception e, ReadOnlyMemory<byte> data = default);
}