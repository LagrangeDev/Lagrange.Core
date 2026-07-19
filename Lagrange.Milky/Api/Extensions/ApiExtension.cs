using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.Milky.Api.Extensions;

public static partial class ApiExtension
{
    public static partial IServiceCollection AddApiHandlers(this IServiceCollection services);
}