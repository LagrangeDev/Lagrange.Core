using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class GetGroupMessageEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public uint StartSequence { get; }
    
    public uint EndSequence { get; }

    public List<MessageChain> Chains { get; } = new();

    private GetGroupMessageEvent(uint groupUin, uint startSequence, uint endSequence) : base(true)
    {
        GroupUin = groupUin;
        StartSequence = startSequence;
        EndSequence = endSequence;
    }

    private GetGroupMessageEvent(int resultCode, List<MessageChain> chains) : base(resultCode)
    {
        Chains = chains;
    }

    public static GetGroupMessageEvent Create(uint groupUin, uint startSequence, uint endSequence)
        => new(groupUin, startSequence, endSequence);

    public static GetGroupMessageEvent Result(int resultCode, List<MessageChain> chains)
        => new(resultCode, chains);
}