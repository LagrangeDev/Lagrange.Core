using System.Net.Sockets;

namespace Lagrange.OneBot.Core.Network;

/// <summary>
/// Provide a simple WebSocket client and server
/// </summary>
internal class WebSocket : IDisposable
{
    private readonly Socket _socket;

    private readonly Thread _receiveLoop;
    
    public bool IsConnected => _socket.Connected;
    
    public event MessageReceivedEventHandler? OnMessageReceived;
    
    public delegate void MessageReceivedEventHandler(WebSocket sender, byte[] data);

    public WebSocket()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _receiveLoop = new Thread(() =>
        {
            while (_socket.Connected)
            {
                var buffer = new byte[1024 * 1024 * 64]; // max 64MB
                int length = _socket.Receive(buffer);
                if (length > 0)
                {
                    var data = buffer[..length];
                    OnMessageReceived?.Invoke(this, data);
                }
            }
        });
    }
    
    public async Task<bool> ConnectAsync(string host, int port)
    {
        await _socket.ConnectAsync(host, port);
        return true;
    }
    
    public async Task<bool> SendAsync(byte[] data)
    {
        await _socket.SendAsync(data, SocketFlags.None);
        return true;
    }

    public void Dispose()
    {
        _socket.Disconnect(false); // The thread will exit automatically
        _socket.Dispose();
        GC.SuppressFinalize(this);
    }
}