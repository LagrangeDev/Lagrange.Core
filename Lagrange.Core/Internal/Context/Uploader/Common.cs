using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Context.Uploader;

internal static class Common
{
    public static async Task<byte[]> GetTicket(ContextCollection context)
    {
        var hwUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResult = await context.Business.SendEvent(hwUrlEvent);
        return ((HighwayUrlEvent)highwayUrlResult[0]).SigSession;
    }
    
    public static NTHighwayNetwork Convert(List<IPv4> ipv4s) => new()
    {
        IPv4s = ipv4s.Select(x => new NTHighwayIPv4
        {
            Domain = new NTHighwayDomain
            {
                IsEnable = true,
                IP = ConvertIP(x.OutIP)
            },
            Port = x.OutPort
        }).ToList()
    };

    private static string ConvertIP(uint raw)
    {
        var ip = BitConverter.GetBytes(raw);
        return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
    }
}