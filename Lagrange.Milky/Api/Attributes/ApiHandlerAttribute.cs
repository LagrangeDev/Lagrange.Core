using System;
using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.Milky.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApiHandlerAttribute(string name) : Attribute
{
    public string Name { get; } = name;
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Singleton;
}