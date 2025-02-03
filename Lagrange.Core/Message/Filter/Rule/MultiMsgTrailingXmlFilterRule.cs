
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message.Filter.Rule;

public class MultiMsgTrailingXmlFilterRule : IMessageFilterRule
{
    public IEnumerable<int> Apply(MessageChain chain)
    {
        if (chain.Count != 2) return Array.Empty<int>();
        if (chain[0] is not MultiMsgEntity) return Array.Empty<int>();
        if (chain[1] is not XmlEntity xml) return Array.Empty<int>();
        if (xml.ServiceId != 35) return Array.Empty<int>();
        return new int[] { 1 };
    }
}