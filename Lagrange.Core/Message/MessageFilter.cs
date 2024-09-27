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
    private static readonly List<Func<MessageChain, List<int>>> FilterRules = new();

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
                    if (forward.TargetUin == mention.Uin) return new List<int>() { x.IndexOf(mention) };
                }
            }

            return new List<int>();
        });

        FilterRules.Add(x =>
        {
            var result = new List<int>();

            var images = x.OfType<ImageEntity>().ToArray();

            for (int i = 0; i < images.Length; i++)
            {
                var imageOld = images[i];
                if (!Uri.IsWellFormedUriString(imageOld.ImageUrl, UriKind.RelativeOrAbsolute))
                {
                    result.Add(x.IndexOf(imageOld));
                    continue;
                }
                
                var uri = new Uri(imageOld.ImageUrl);
                if (uri.Host == "gchat.qpic.cn")
                {
                    for (int j = 0; j < images.Length; j++)
                    {
                        if (imageOld.FilePath == images[i].FilePath)
                        {
                            result.Add(x.IndexOf(imageOld));
                            break;
                        }
                    }
                }
            }

            return result;
        });
    }

    public static void Filter(MessageChain chain)
    {
        foreach (var rule in FilterRules)
        {
            List<int> indexs = rule(chain);
            foreach (var index in indexs)
            {
                chain.RemoveAt(index);
            }
        }
    }
}