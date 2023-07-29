using System.Buffers.Binary;
using System.Net;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Network;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Network;

namespace Lagrange.Core.Core.Context;

/// <summary>
/// <para>Provide Low-Allocation Tcp Client which connects to the Tencent's SSO Server</para>
/// <para>Internal Implementation, Packet Received would be dispatched to <see cref="PacketContext"/> for decryption and unpack</para>
/// <para>MSF Service is also implemented here</para>
/// </summary>
internal class SocketContext : ContextBase, IClientListener
{
    private const string Tag = nameof(SocketContext);
    
    private readonly ClientListener _tcpClient;

    private readonly bool _reconnect;

    private Uri? ServerUri { get; set; }
    
    public uint HeaderSize => 4;

    public bool Connected => _tcpClient.Connected;
    
    public SocketContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, bool reconnect) 
        : base(collection, keystore, appInfo, device)
    {
        _tcpClient = new CallbackClientListener(this);
        _reconnect = reconnect;
    }

    public async Task<bool> Connect(bool useIPv6Network = false)
    {
        if (_tcpClient.Connected) return true;

        var servers = await OptimumServer(true, useIPv6Network);
        ServerUri = servers.First();
        return await _tcpClient.Connect(ServerUri.Host, ServerUri.Port);
    }
    
    public async Task<bool> Reconnect()
    {
        if (ServerUri != null) return await _tcpClient.Connect(ServerUri.Host, ServerUri.Port);

        return false;
    }
    
    public async Task<bool> Send(byte[] packet) => await _tcpClient.Send(packet);

    public uint GetPacketLength(ReadOnlySpan<byte> header) => BinaryPrimitives.ReadUInt32BigEndian(header);

    public void OnRecvPacket(ReadOnlySpan<byte> packet)
    {
        var binary = new BinaryPacket(packet.ToArray());
        Collection.Packet.DispatchPacket(binary);
    }

    public void OnDisconnect()
    {
        if (_reconnect) _ = Reconnect();
    }

    public void OnSocketError(Exception e)
    {
        Console.WriteLine(e);
    }
    
    private static readonly Uri[] MsfUris =
    {
        new("http://msfwifi.3g.qq.com:8080"), // IPv4
        new("http://msfwifiv6.3g.qq.com:14000") // IPv6
    };
    
    private static readonly Uri[] HardCodeIPv4Uris = 
    {
        new("http://msfwifi.3g.qq.com:8080"),
        new("http://163.177.89.195:14000"),
        new("http://120.232.18.27:443"),
        new("http://157.255.13.77:443"),
        new("http://140.207.123.177:8080"),
        new("http://221.198.69.96:443"),
        new("http://123.150.76.143:14000"),
        new("http://183.3.235.162:443"),
        new("http://61.129.6.101:8080"),
        new("http://42.81.169.100:443"),
        new("http://183.232.94.44:14000"),
        new("http://msfxg.3g.qq.com:80"),
        new("http://117.144.244.33:8080"),
        new("http://111.30.138.152:443"),
        new("http://203.205.255.221:14000"),
        new("http://203.205.255.224:443"),
        new("http://183.3.235.162:8080"),
        new("http://183.47.102.193:8080")
    };

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

        var result = requestMsf ? await ResolveDns() : useIPv6Network ? HardCodeIPv6Uris : TestIPv4HardCodes;
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
        var addresses = await Dns.GetHostAddressesAsync(dns);
        var result = new Uri[addresses.Length];
        
        for (int i = 0; i < addresses.Length; i++) result[i] = new Uri($"http://{addresses[i]}:8080");

        return result;
    }
}