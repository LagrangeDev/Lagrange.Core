using Lagrange.Core.Message;
using static Lagrange.Core.Internal.Service.Message.PushMessageService;

namespace Lagrange.Core.Internal.Event.Message;

internal class PushMessageEvent : ProtocolEvent
{
    public MessageType Type { get; set; }

    public MessageChain Chain { get; set; }

    private PushMessageEvent(int resultCode, MessageChain chain, PkgType packetType) : base(resultCode)
    {
        Chain = chain;
        switch (packetType)
        {
            case PkgType.PrivateMessage:
            case PkgType.PrivateRecordMessage:
            case PkgType.PrivateFileMessage:
                Type = MessageType.Friend;
                break;
            case PkgType.GroupMessage:
                Type = MessageType.Group;
                break;
            case PkgType.TempMessage:
                Type = MessageType.Temp;
                break;
        }
    }

    public static PushMessageEvent Create(MessageChain chain, PkgType packetType) => new(0, chain, packetType);

    public enum MessageType
    {
        Friend,
        Group,
        Temp,
    }
}