using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message;

/// <summary>
/// MessageFilter is a static class that provides a set of rules to filter the message chain.
/// </summary>
internal static class MessageFilter
{
    /// <summary>
    /// The filter rules, result of the predicate is the index of the message entity that should be removed, -1 means the message entity should be kept.
    /// </summary>
    private static readonly List<Func<MessageChain, int>> FilterRules = new();

    static MessageFilter()
    { 
        FilterRules.Add(x =>
        {
            var forwards = x.OfType<ForwardEntity>().ToArray();
            var mentions = x.OfType<MentionEntity>().ToArray();
            
            foreach (var forward in forwards)
            {
                foreach (var mention in mentions)
                {
                    if (forward.TargetUin == mention.Uin) return x.IndexOf(mention);
                }
            }
            
            return -1;
        });
    }
        
    public static void Filter(MessageChain chain)
    {
        foreach (var rule in FilterRules)
        {
            int index = rule(chain);
            if (index != -1) chain.RemoveAt(index);
        }
    }
}