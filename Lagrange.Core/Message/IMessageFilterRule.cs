namespace Lagrange.Core.Message;

internal interface IMessageFilterRule
{
    IEnumerable<int> Handle(MessageChain chain);
}