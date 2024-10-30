using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
[EventSubscribe(typeof(GroupAiRecordEvent))]
[Service("OidbSvcTrpcTcp.0x929b_0")]
internal class GroupAiRecordService : BaseService<GroupAiRecordEvent>
{
    protected override bool Build(GroupAiRecordEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output,
        out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929B_0>(
            new OidbSvcTrpcTcp0x929B_0() { Group = input.GroupUin, tts = input.Character, Msg = input.Text }
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

        var index = payload.Body.MsgInfo.MsgInfoBody[0].Index;

        output = GroupAiRecordEvent.Result((int)payload.ErrorCode,
            new RecordEntity(index.FileUuid, index.Info.FileName, index.Info.FileHash.UnHex())
            {
                AudioLength = (int)index.Info.Time, FileSha1 = index.Info.FileSha1, MsgInfo = payload.Body.MsgInfo
            });
        return true;
    }
}