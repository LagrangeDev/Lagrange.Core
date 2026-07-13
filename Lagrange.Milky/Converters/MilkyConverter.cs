using Lagrange.Core;
using Lagrange.Milky.Caching;

namespace Lagrange.Milky.Converters;

public partial class MilkyConverter(BotContext lagrange, MessageCache cache)
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MessageCache _cache = cache;
}

