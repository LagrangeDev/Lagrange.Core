using System.Net.NetworkInformation;

namespace Lagrange.Core.Utility.Network;

internal static class Icmp
{
    public static async Task<long> PingAsync(Uri hostIp, int timeout = 1000)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(hostIp.Host, timeout);
            return reply?.RoundtripTime ?? long.MaxValue;
        }
        catch (PlatformNotSupportedException)
        {
            return long.MaxValue;
        }
    }
}