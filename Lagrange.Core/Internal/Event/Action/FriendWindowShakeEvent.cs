using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Action;

internal class FriendWindowShakeEvent : ProtocolEvent
{
    public uint FriendUin { get; set; }
    public MessageResult? MessageResult { get; set; }

    private FriendWindowShakeEvent(uint friendUin) : base(true)
    {
        FriendUin = friendUin;
        MessageResult = null;
    }

    private FriendWindowShakeEvent(int resultCode, MessageResult? result) : base(resultCode)
    {
        MessageResult = result;
    }

    public static FriendWindowShakeEvent Create(uint friendUin) => new(friendUin);

    public static FriendWindowShakeEvent Result(int resultCode, MessageResult? result) => new(resultCode, result);
}
