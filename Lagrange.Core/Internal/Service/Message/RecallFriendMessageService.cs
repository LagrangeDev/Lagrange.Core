using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(RecallFriendMessageEvent))]
[Service("trpc.msg.msg_svc.MsgService.SsoC2CRecallMsg")]
internal class RecallFriendMessageService : BaseService<RecallFriendMessageEvent>
{
    protected override bool Build(RecallFriendMessageEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new C2CRecallMsg
        {
            Type = 1,
            TargetUid = input.TargetUid,
            Info = new C2CRecallMsgInfo
            {
                ClientSequence = input.ClientSeq,
                Random = input.Random,
                MessageId = 0x1000000UL << 32 | input.Random,
                Timestamp = input.Timestamp,
                Field5 = 0,
                MessageSequence = input.MessageSeq
            },
            Settings = new C2CRecallMsgSettings
            {
                Field1 = false,
                Field2 = false
            },
            Field6 = false
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out RecallFriendMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = RecallFriendMessageEvent.Result(0); // 腾讯自己都不知道有没有这条消息 都不会报错的 应该是交给客户端处理了
        extraEvents = null;
        return true;
    }
}