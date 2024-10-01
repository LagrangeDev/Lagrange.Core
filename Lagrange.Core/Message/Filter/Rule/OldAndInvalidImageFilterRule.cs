
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message.Filter.Rule;

internal class OldAndInvalidImageFilterRule : IMessageFilterRule
{
    public IEnumerable<int> Apply(MessageChain chain)
    {
        var images = chain.OfType<ImageEntity>().ToArray();

        if (images.Length == 0) return Array.Empty<int>();

        var result = new List<int>();
        var groups = images.GroupBy((image) =>
            {
                if (!Uri.IsWellFormedUriString(image.ImageUrl, UriKind.RelativeOrAbsolute)) return ImageType.Invalid;

                Uri uri = new Uri(image.ImageUrl);

                if (uri.Host == "gchat.qpic.cn") return ImageType.Old;

                if (uri.Host == "multimedia.nt.qq.com.cn") return ImageType.New;

                return ImageType.Other;
            })
            .ToDictionary((group) => group.Key, Enumerable.ToList);

        if (groups.ContainsKey(ImageType.Invalid)) result.AddRange(groups[0].Select(image => chain.IndexOf(image)));

        if (!groups.ContainsKey(ImageType.Old)) return result;

        if (!groups.ContainsKey(ImageType.New)) return result;

        var foundImages = groups[ImageType.Old].Where(old =>
            {
                var @new = groups[ImageType.New].Find(@new => old.ImageMd5.SequenceEqual(@new.ImageMd5));
                bool isFound = @new != null;
                if (isFound) groups[ImageType.New].Remove(@new);
                return isFound;
            });
        result.AddRange(foundImages.Select(image => chain.IndexOf(image)));

        return result;
    }

    private enum ImageType
    {
        Invalid,
        Old,
        New,
        Other
    }
}