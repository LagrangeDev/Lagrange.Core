using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class GetC2cMessageEvent : ProtocolEvent
{
    public string FriendUid { get; } = string.Empty;

    public ulong StartSequence { get; }

    public ulong EndSequence { get; }

    public List<MessageChain> Chains { get; } = new();

    private GetC2cMessageEvent(string friendUid, ulong startSequence, ulong endSequence) : base(true)
    {
        FriendUid = friendUid;
        StartSequence = startSequence;
        EndSequence = endSequence;
    }

    private GetC2cMessageEvent(int retcode, List<MessageChain> chains) : base(retcode)
    {
        Chains = chains;
    }

    public static GetC2cMessageEvent Create(string friendUid, ulong startSequence, ulong endSequence)
        => new(friendUid, startSequence, endSequence);

    public static GetC2cMessageEvent Result(int retcode, List<MessageChain> chains)
        => new(retcode, chains);
}