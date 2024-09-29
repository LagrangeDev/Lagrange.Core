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
    private static readonly List<Func<MessageChain, (int index, bool isCompleted)>> FilterRules = new();

    static MessageFilter()
    {
        FilterRules.Add(chain =>
        {
            var forwardIndex = chain.FindIndex(entity => entity is ForwardEntity);

            if (chain[forwardIndex + 1] is MentionEntity mention) return (forwardIndex + 1, true);

            return (-1, true);
        });

        FilterRules.Add(x =>
        {
            var images = x.OfType<ImageEntity>().ToArray();

            for (int i = 0; i < images.Length; i++)
            {
                var imageOld = images[i];
                if (!Uri.IsWellFormedUriString(imageOld.ImageUrl, UriKind.RelativeOrAbsolute))
                {
                    return (x.IndexOf(imageOld), false);
                }

                var uri = new Uri(imageOld.ImageUrl);
                if (uri.Host == "gchat.qpic.cn")
                {
                    for (int j = 0; j < images.Length; j++)
                    {
                        if (imageOld.FilePath == images[i].FilePath)
                        {
                            return (x.IndexOf(imageOld), false);
                        }
                    }
                }
            }

            return (-1, true);
        });
    }

    public static void Filter(MessageChain chain)
    {
        foreach (var rule in FilterRules)
        {
            for ((int index, bool isCompleted) = rule(chain); index != -1; (index, isCompleted) = rule(chain))
            {
                chain.RemoveAt(index);

                if (isCompleted) break;
            }
        }
    }
}