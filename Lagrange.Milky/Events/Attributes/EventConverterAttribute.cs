using System;
using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.Milky.Events.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EventConverterAttribute : Attribute
{
    public long Priority { get; init; } = 0;
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Singleton;
}