using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(GetStrangerGroupInfoEvent))]
[Service("OidbSvcTrpcTcp.0x88d_110")]
internal class GetStrangerGroupInfoService : BaseService<GetStrangerGroupInfoEvent>
{
    protected override bool Build(GetStrangerGroupInfoEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x88D>(new OidbSvcTrpcTcp0x88D
        {
            Field1 = (uint)Random.Shared.NextInt64(),
            Config = new OidbSvcTrpcTcp0x88DConfig
            {
                Uin = input.Uin,
                Flags = new OidbSvcTrpcTcp0x88DFlags
                {
                    CreateTime = true,
                    MaxMemberCount = true,
                    MemberCount = true,
                    Name = "",
                    Uin = true,
                }
            }
        }, 0x88d, 110);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GetStrangerGroupInfoEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x88D_0Response>>(input);

        if (payload.ErrorCode == 0)
        {
            output = GetStrangerGroupInfoEvent.Result(0, null, new BotStrangerGroupInfo
            {
                CreateTime = payload.Body.GroupInfo.Results.CreateTime,
                MaxMemberCount = payload.Body.GroupInfo.Results.MaxMemberCount,
                MemberCount = payload.Body.GroupInfo.Results.MemberCount,
                Name = payload.Body.GroupInfo.Results.Name,
                Uin = payload.Body.GroupInfo.Results.Uin,
            });
        }
        else
        {
            output = GetStrangerGroupInfoEvent.Result((int)payload.ErrorCode, payload.ErrorMsg, new());
        }
        extraEvents = null;
        return true;
    }
}