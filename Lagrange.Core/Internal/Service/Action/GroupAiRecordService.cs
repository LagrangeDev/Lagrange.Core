using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupAiRecordEvent))]
[Service("OidbSvcTrpcTcp.0x929b_0")]
internal class GroupAiRecordService : BaseService<GroupAiRecordEvent>
{
    protected override bool Build(GroupAiRecordEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output,
        out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929B_0>(
            new OidbSvcTrpcTcp0x929B_0
            {
                GroupCode = input.GroupUin,
                VoiceId = input.Character,
                Text = input.Text,
                ChatType = input.ChatType,
                ClientMsgInfo = new OidbSvcTrpcTcp0x929B_0ClientMsgInfo
                {
                    MsgRandom = input.ChatId
                }
            }
        );
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupAiRecordEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929B_0Response>>(input);

        extraEvents = null;
        if (payload.ErrorCode != 0)
        {
            output = GroupAiRecordEvent.Result((int)payload.ErrorCode, payload.ErrorMsg);
        }
        else
        {
            output = GroupAiRecordEvent.Result(0, payload.Body.MsgInfo);
        }


        return true;
    }
}