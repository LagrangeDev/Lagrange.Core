using System.Collections;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Message.FilterRule;

namespace Lagrange.Core.Message;

/// <summary>
/// MessageFilter is a static class that provides a set of rules to filter the message chain.
/// </summary>
internal static class MessageFilter
{
    /// <summary>
    /// The filter rules, result of the predicate is the index of the message entity that should be removed, -1 means the message entity should be kept.
    /// </summary>
    private static readonly List<IMessageFilterRule> _rules = new();

    static MessageFilter()
    {
        _rules.Add(new ForwardRule());
        _rules.Add(new ImageRule());
    }

    public static void Filter(MessageChain chain)
    {
        foreach (var rule in _rules)
        {
            foreach (var index in rule.Handle(chain).OrderByDescending(key => key))
            {
                chain.RemoveAt(index);
            }
        }
    }
}