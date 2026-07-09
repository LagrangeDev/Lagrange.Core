using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<PushMessageEvent>(Protocols.All)]
[Service("trpc.msg.olpush.OlPushService.MsgPush")]
internal class PushMessageService : BaseService<PushMessageEvent, PushMessageEvent>
{
    protected override ValueTask<PushMessageEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var msg = ProtoHelper.Deserialize<MsgPush>(input.Span);
        
        return new ValueTask<PushMessageEvent>(new PushMessageEvent(msg, input));
    }
}