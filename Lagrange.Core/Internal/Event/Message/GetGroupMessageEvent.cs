using Lagrange.Core.Message;
using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class GetGroupMessageEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public uint StartSequence { get; }
    
    public uint EndSequence { get; }

    public List<MessageChain> Chains { get; } = new();

    public List<PushMsgBody> PushMsgBodies { get; } = new();

    private GetGroupMessageEvent(uint groupUin, uint startSequence, uint endSequence) : base(true)
    {
        GroupUin = groupUin;
        StartSequence = startSequence;
        EndSequence = endSequence;
    }

    private GetGroupMessageEvent(int resultCode, List<MessageChain> chains, List<PushMsgBody> bodies) : base(resultCode)
    {
        Chains = chains;
        PushMsgBodies = bodies;
    }

    public static GetGroupMessageEvent Create(uint groupUin, uint startSequence, uint endSequence)
        => new(groupUin, startSequence, endSequence);

    public static GetGroupMessageEvent Result(int resultCode, List<MessageChain> chains, List<PushMsgBody> bodies)
        => new(resultCode, chains, bodies);
}