using System.Collections.Frozen;
using Lagrange.Core.Common;
using Lagrange.Core.Events;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Logic;

[EventSubscribe<PushMessageEvent>(Protocols.All)]
internal class PushLogic(BotContext ctx) : ILogic
{
    private readonly FrozenDictionary<MsgMatchKey, List<MsgPushProcessorBase>> _processors = MsgPushProcessorRegistry.Create();

    public async ValueTask Incoming(ProtocolEvent e)
    {
        if (e is not PushMessageEvent msgEvt) return;
        var msgType = (MsgType)msgEvt.MsgPush.CommonMessage.ContentHead.Type;
        var subType = msgEvt.MsgPush.CommonMessage.ContentHead.SubType;
        var hasContent = msgEvt.MsgPush.CommonMessage.MessageBody?.MsgContent is not null;
        var content = msgEvt.MsgPush.CommonMessage.MessageBody?.MsgContent;

        if (_processors.TryGetValue(new MsgMatchKey(msgType, subType, hasContent), out var handlers))
        {
            foreach (var handler in handlers)
            {
                if (await handler.Handle(ctx, msgType, subType, msgEvt, content))
                    return;
            }
        }

        if (_processors.TryGetValue(new MsgMatchKey(msgType, -1, hasContent), out handlers))
        {
            foreach (var handler in handlers)
            {
                if (await handler.Handle(ctx, msgType, subType, msgEvt, content))
                    return;
            }
        }
    }
}

internal enum MsgType
{
    GroupMemberIncreaseNotice = 33,
    GroupMemberDecreaseNotice = 34,
    GroupMessage = 82,
    GroupJoinNotification = 84,
    TempMessage = 141,
    PrivateMessage = 166,
    Event0x20D = 525,
    Event0x210 = 528,  // friend related event
    Event0x2DC = 732,  // group related event
}

internal readonly record struct MsgMatchKey(MsgType MsgType, int SubType = -1, bool RequireContent = false);

internal abstract class MsgPushProcessorBase
{
    internal abstract ValueTask<bool> Handle(BotContext context, MsgType msgType, int subType, PushMessageEvent msgEvt, ReadOnlyMemory<byte>? content);
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class MsgPushProcessorAttribute : Attribute
{
    public MsgType MsgType { get; init; }
    public int SubType { get; }
    public bool RequireContent { get; } = false;

    public MsgPushProcessorAttribute(MsgType msgType, bool requireContent = false)
        : this(msgType, -1, requireContent) { }

    public MsgPushProcessorAttribute(MsgType msgType, int subType, bool requireContent = false)
    {
        MsgType = msgType;
        SubType = subType;
        RequireContent = requireContent;
    }
}
