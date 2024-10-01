namespace Lagrange.Core.Message.Filter;

internal interface IMessageFilterRule
{
    IEnumerable<int> Handle(MessageChain chain);
}