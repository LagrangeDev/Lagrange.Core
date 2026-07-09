using Lagrange.Core.Common;
using Lagrange.Core.Events;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<SendMessageEventReq>(Protocols.All)]
[EventSubscribe<SendFriendFileEventReq>(Protocols.All)]
[Service("MessageSvc.PbSendMsg")]
internal class SendMessageService : BaseService<ProtocolEvent, ProtocolEvent>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(ProtocolEvent input, BotContext context)
    {
        switch (input)
        {
            case SendMessageEventReq sendMsg:
            {
                var payload = MessagePacker.Build(sendMsg.Message);
                return new ValueTask<ReadOnlyMemory<byte>>(payload);
            }
            case SendFriendFileEventReq file:
            {
                var payload = MessagePacker.BuildTrans0X211(file.Friend, file.Request, file.Response, file.ClientSequence, file.Sequence);
                return new ValueTask<ReadOnlyMemory<byte>>(payload);
            }
        }
        
        throw new NotSupportedException($"Unsupported input type: {input.GetType()}");
    }

    protected override ValueTask<ProtocolEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var response = ProtoHelper.Deserialize<PbSendMsgResp>(input.Span);
        ulong sequence = response.ClientSequence == 0 ? response.Sequence : response.ClientSequence;
        return new ValueTask<ProtocolEvent>(new SendMessageEventResp(response.Result, response.SendTime, sequence));
    }
}