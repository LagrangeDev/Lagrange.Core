namespace Lagrange.Core.Message.Filter;

internal interface IMessageFilterRule
{
    IEnumerable<int> Apply(MessageChain chain);
}