using Lagrange.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.Milky.Events.Extensions;

public static partial class EventExtension
{
    public static partial void RegisterConvertibleEvents(this BotContext lagrange, IGenericEventHandler handler);
    public static partial void UnregisterConvertibleEvents(this BotContext lagrange, IGenericEventHandler handler);
    public static partial IServiceCollection AddEventConverters(this IServiceCollection services);
}