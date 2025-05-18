
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message.Filter.Rule;

public class AtBeforeForwardInOldClient : IMessageFilterRule
{
    public IEnumerable<int> Apply(MessageChain chain)
    {
        if (chain.Count < 3) return Array.Empty<int>();
        if (chain[0] is not MentionEntity mention) return Array.Empty<int>();
        if (chain[1] is not TextEntity { Text: " " }) return Array.Empty<int>();
        if (chain[2] is not ForwardEntity forward) return Array.Empty<int>();
        if (mention.Uin != forward.TargetUin) return Array.Empty<int>();

        return new int[] { 0, 1 };
    }
}