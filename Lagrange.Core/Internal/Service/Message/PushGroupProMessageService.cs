using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Message;
using static Lagrange.Core.Message.MessageChain;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(PushMessageEvent))]
[Service("MsgPush.PushGroupProMsg")]
internal class PushGroupProMessageService : BaseService<PushMessageEvent>
{
    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out PushMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var message = Serializer.Deserialize<PushGroupProMsg>(input);
        var chain = MessagePacker.Parse(message.Message);
        // patch, what i can say
        chain.Type = MessageType.GroupPro;
        chain.GroupMemberInfo = new BotGroupMember();
        chain.GroupMemberInfo.MemberName = message.Message.Unknown4?.SenderNickName ?? "";
        chain.GroupMemberInfo.Uin = (uint)(message.Message.Unknown1.Field1?.SenderId ?? 0 % ((ulong)uint.MaxValue + 1));
        output = PushMessageEvent.Create(chain);
        extraEvents = new List<ProtocolEvent>();
        return true;
    }
}
