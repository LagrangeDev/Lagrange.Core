using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class GetRoamMessageEvent : ProtocolEvent
{
    public string FriendUid { get; } = string.Empty;
    
    public uint Time { get; }
    
    public uint Count { get; }

    public List<MessageChain> Chains { get; } = new();

    private GetRoamMessageEvent(string friendUid, uint time, uint count) : base(true)
    {
        FriendUid = friendUid;
        Time = time;
        Count = count;
    }

    private GetRoamMessageEvent(int resultCode, List<MessageChain> chains) : base(resultCode)
    {
        Chains = chains;
    }

    public static GetRoamMessageEvent Create(string friendUid, uint time, uint count)
        => new(friendUid, time, count);

    public static GetRoamMessageEvent Result(int resultCode, List<MessageChain> chains)
        => new(resultCode, chains);
}