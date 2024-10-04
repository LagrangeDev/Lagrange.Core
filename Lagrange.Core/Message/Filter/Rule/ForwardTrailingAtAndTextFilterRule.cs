
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message.Filter.Rule;

internal class ForwardTrailingAtAndTextFilterRule : IMessageFilterRule
{
    public IEnumerable<int> Apply(MessageChain chain)
    {
        if (chain.Type != MessageChain.MessageType.Group) return Array.Empty<int>();

        if (chain.Count < 2) return Array.Empty<int>();

        if (chain[0] is not ForwardEntity forward) return Array.Empty<int>();

        if (chain[1] is not MentionEntity mention) return Array.Empty<int>();
        if (mention.Uin != forward.TargetUin) return Array.Empty<int>();

        if (chain.Count == 2) return new int[] { 1 };
        if (chain[2] is not TextEntity { Text: " " }) return new int[] { 1 };
        return new int[] { 1, 2 };
    }
}