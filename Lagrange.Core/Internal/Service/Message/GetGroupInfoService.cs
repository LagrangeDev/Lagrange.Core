using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(GetGroupInfoEvent))]
[Service("OidbSvcTrpcTcp.0x88d_0")]
internal class GetGroupInfoService : BaseService<GetGroupInfoEvent>
{
    protected override bool Build(GetGroupInfoEvent input, BotKeystore keystore, BotAppInfo appInfo,
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
                    OwnerUid = true,
                    CreateTime = true,
                    MaxMemberCount = true,
                    MemberCount = true,
                    Level = true,
                    Name = "",
                    NoticePreview = "",
                    Uin = true,
                    LastSequence = true,
                    LastMessageTime = true,
                    Question = true,
                    Answer = "",
                    MaxAdminCount = "",
                }
            }
        }, 0x88d, 0);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GetGroupInfoEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x88D_0Response>>(input);

        if (payload.ErrorCode == 0)
        {
            output = GetGroupInfoEvent.Result(0, null, new BotGroupInfo
            {
                OwnerUid = payload.Body.GroupInfo.Results.OwnerUid,
                CreateTime = payload.Body.GroupInfo.Results.CreateTime,
                MaxMemberCount = payload.Body.GroupInfo.Results.MaxMemberCount,
                MemberCount = payload.Body.GroupInfo.Results.MemberCount,
                Level = payload.Body.GroupInfo.Results.Level,
                Name = payload.Body.GroupInfo.Results.Name,
                NoticePreview = payload.Body.GroupInfo.Results.NoticePreview,
                Uin = payload.Body.GroupInfo.Results.Uin,
                LastSequence = payload.Body.GroupInfo.Results.LastSequence,
                LastMessageTime = payload.Body.GroupInfo.Results.LastMessageTime,
                Question = payload.Body.GroupInfo.Results.Question,
                Answer = payload.Body.GroupInfo.Results.Answer,
                MaxAdminCount = payload.Body.GroupInfo.Results.MaxAdminCount,
            });
        }
        else
        {
            output = GetGroupInfoEvent.Result((int)payload.ErrorCode, payload.ErrorMsg, new());
        }
        extraEvents = null;
        return true;
    }
}