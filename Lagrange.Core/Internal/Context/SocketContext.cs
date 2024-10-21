using System.Buffers.Binary;
using System.Net;
using Lagrange.Core.Common;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Internal.Network;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;

namespace Lagrange.Core.Internal.Context;

/// <summary>
/// <para>Provide Low-Allocation Tcp Client which connects to the Tencent's SSO Server</para>
/// <para>Internal Implementation, Packet Received would be dispatched to <see cref="PacketContext"/> for decryption and unpack</para>
/// <para>MSF Service is also implemented here</para>
/// </summary>
internal class SocketContext : ContextBase, IClientListener
{
    private const string Tag = nameof(SocketContext);
    
    private readonly ClientListener _tcpClient;

    private readonly BotConfig _config;

    private Uri? ServerUri { get; set; }
    
    public uint HeaderSize => 4;

    public bool Connected => _tcpClient.Connected;
    
    public SocketContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, BotConfig config) 
        : base(collection, keystore, appInfo, device)
    {
        _tcpClient = new CallbackClientListener(this);
        _config = config;
    }

    public async Task<bool> Connect()
    {
        if (_tcpClient.Connected) return true;

        var servers = await OptimumServer(_config.GetOptimumServer, _config.UseIPv6Network);
        ServerUri = servers.First();
        return await _tcpClient.Connect(ServerUri.Host, ServerUri.Port);
    }
    
    private async Task<bool> Reconnect()
    {
        if (ServerUri != null && !_tcpClient.Connected)
        {
            bool reconnect = await _tcpClient.Connect(ServerUri.Host, ServerUri.Port);
            if (reconnect)
            {
                Collection.Log.LogInfo(Tag, $"Reconnect to {ServerUri}");
                await Collection.Business.WtExchangeLogic.BotOnline(BotOnlineEvent.OnlineReason.Reconnect);
            }
        }

        return false;
    }

    public void Disconnect() => _tcpClient.Disconnect();

    public Task<bool> Send(ReadOnlyMemory<byte> packet) => _tcpClient.Send(packet);

    public uint GetPacketLength(ReadOnlySpan<byte> header) => BinaryPrimitives.ReadUInt32BigEndian(header);

    public void OnRecvPacket(ReadOnlySpan<byte> packet)
    {
        var binary = new BinaryPacket(packet.ToArray());
        Collection.Packet.DispatchPacket(binary);
    }

    public void OnDisconnect()
    {
        Collection.Log.LogFatal(Tag, "Socket Disconnected, Scheduling Reconnect");
        
        if (_config.AutoReconnect)
        {
            Collection.Scheduler.Interval("Reconnect", 10 * 1000, async () =>
            {
                if (await Reconnect()) Collection.Scheduler.Cancel("Reconnect");
            });
        }
    }

    public void OnSocketError(Exception e, ReadOnlyMemory<byte> data = default)
    {
        Collection.Log.LogFatal(Tag, $"Socket Error: {e.Message}");
        if (e.StackTrace != null) Collection.Log.LogFatal(Tag, e.StackTrace);
        if (data.Length > 0) Collection.Log.LogDebug(Tag, $"Data: {data.Span.Hex()}");

        _tcpClient.Disconnect();
        if (!_tcpClient.Connected) OnDisconnect();
    }

    private static readonly Uri[] HardCodeIPv6Uris = 
    {
        new("http://msfwifiv6.3g.qq.com:14000")
    };

    /// <summary>
    /// 好像这才是真货
    /// </summary>
    private static readonly Uri[] TestIPv4HardCodes =
    {
        new("http://183.47.102.193:8080"),
        new("http://14.22.9.84:8080"),
        new("http://119.147.190.138:8080")
    };
    
    private async Task<List<Uri>> OptimumServer(bool requestMsf, bool useIPv6Network = false)
    {

        var result = requestMsf ? await ResolveDns(useIPv6Network) : useIPv6Network ? HardCodeIPv6Uris : TestIPv4HardCodes;
        var latencyTasks = result.Select(uri => Icmp.PingAsync(uri)).ToArray();
        var latency = await Task.WhenAll(latencyTasks);
        Array.Sort(latency, result);
        
        var list = result.ToList();
        for (int i = 0; i < list.Count; i++) Collection.Log.LogVerbose(Tag, $"Server: {list[i]} Latency: {latency[i]}");
        return list;
    }
    
    private static async Task<Uri[]> ResolveDns(bool useIPv6Network = false)
    {
        string dns = useIPv6Network ? "msfwifiv6.3g.qq.com" : "msfwifi.3g.qq.com";
        var addresses = await Dns.GetHostEntryAsync(dns);
        var result = new Uri[addresses.AddressList.Length];
        
        for (int i = 0; i < addresses.AddressList.Length; i++) result[i] = new Uri($"http://{addresses.AddressList[i]}:8080");

        return result;
    }

    public void Dispose()
    {
        _tcpClient.Disconnect();
    }
}