
using Lagrange.Core.Message.Filter.Rule;

namespace Lagrange.Core.Message.Filter;

/// <summary>
/// MessageFilter is a static class that provides a set of rules to filter the message chain.
/// </summary>
internal static class MessageFilter
{
    /// <summary>
    /// The filter rules, result of the predicate is the index of the message entity that should be removed, -1 means the message entity should be kept.
    /// </summary>
    private static readonly IMessageFilterRule[] _rules;

    static MessageFilter()
    {
        _rules = new IMessageFilterRule[] {
            new ForwardTrailingAtAndTextFilterRule(),
            new OldAndInvalidImageFilterRule(),
        };
    }

    public static void Filter(MessageChain chain)
    {
        foreach (var rule in _rules)
        {
            foreach (var index in rule.Apply(chain).OrderByDescending(key => key))
            {
                chain.RemoveAt(index);
            }
        }
    }
}