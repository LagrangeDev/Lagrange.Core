using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<HighwaySessionEventReq>(Protocols.All)]
[Service("HttpConn.0x6ff_501")]
internal class HighwaySessionService : BaseService<HighwaySessionEventReq, HighwaySessionEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(HighwaySessionEventReq input, BotContext context)
    {
        var payload = new C501ReqBody
        {
            ReqBody = new SubCmd0x501ReqBody
            {
                Uin = 0,
                IdcId = 0,
                Appid = 16,
                LoginSigType = 0,
                LoginSigTicket = context.Keystore.WLoginSigs.A2,
                ServiceTypes = [1, 5, 10, 21],
                RequestFlag = 3,
                Field9 = 2,
                Field10 = 9,
                Field11 = 8,
                Version = "1.0.1"
            }
        };
        
        return ValueTask.FromResult(ProtoHelper.Serialize(payload));
    }

    protected override ValueTask<HighwaySessionEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var packet = ProtoHelper.Deserialize<C501RspBody>(input.Span);
        
        var servers = new Dictionary<uint, List<string>>();
        foreach (var srvAddr in packet.RspBody.Addrs)
        {
            var addresses = new List<string>();
            foreach (var addr in srvAddr.Addrs)
            {
                addresses.Add($"{ProtocolHelper.UInt32ToIPV4Addr(addr.Ip)}:{addr.Port}/cgi-bin/httpconn?htcmd=0x6FF0087&uin={context.Keystore.Uin}");
            }
            
            servers[srvAddr.ServiceType] = addresses;
        }

        return ValueTask.FromResult(new HighwaySessionEventResp(servers, packet.RspBody.SigSession));
    }
}