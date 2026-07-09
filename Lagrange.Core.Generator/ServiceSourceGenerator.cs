using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Lagrange.Core.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class ServiceSourceGenerator : IIncrementalGenerator
{
    private const string ServiceAttributeFullName = "Lagrange.Core.Services.ServiceAttribute";
    private const string ServiceInterfaceFullName = "Lagrange.Core.Services.IService";
    private const string EventSubscribeAttributeNamespace = "Lagrange.Core.Services";
    private const string EventSubscribeAttributeName = "EventSubscribeAttribute";
    private const long DefaultRequestType = 0x0C;
    private const long DefaultEncryptType = 0x01;

    private static readonly SymbolDisplayFormat TypeDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
            ServiceAttributeFullName,
            static (_, _) => true,
            static (context, _) => ToServiceInfo(context)
        );

        var services = candidates.SelectMany(static (service, _) => service.HasValue ? [service.GetValueOrDefault()] : ImmutableArray<ServiceInfo>.Empty);

        context.RegisterSourceOutput(services.Collect(), static (context, services) => Output(context, services));
    }

    private static ServiceInfo? ToServiceInfo(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol is not INamedTypeSymbol { TypeKind: TypeKind.Class } typeSymbol || typeSymbol.IsAbstract)  return null;

        var serviceInterface = context.SemanticModel.Compilation.GetTypeByMetadataName(ServiceInterfaceFullName);
        if (serviceInterface is null || !typeSymbol.AllInterfaces.Any(type =>
                SymbolEqualityComparer.Default.Equals(type, serviceInterface))) return null;

        var serviceAttribute = context.Attributes[0];
        if (serviceAttribute.ConstructorArguments.Length == 0 || serviceAttribute.ConstructorArguments[0].Value is not string command)  return null;

        long requestType = serviceAttribute.ConstructorArguments.Length > 1
            ? ToInt64(serviceAttribute.ConstructorArguments[1].Value, DefaultRequestType)
            : DefaultRequestType;

        long encryptType = serviceAttribute.ConstructorArguments.Length > 2
            ? ToInt64(serviceAttribute.ConstructorArguments[2].Value, DefaultEncryptType)
            : DefaultEncryptType;

        bool disableLog = serviceAttribute.NamedArguments.Any(static pair => pair is { Key: "DisableLog", Value.Value: true });

        var events = ImmutableArray.CreateBuilder<EventSubscriptionInfo>();
        foreach (var attribute in typeSymbol.GetAttributes())
        {
            if (TryGetEventSubscription(attribute, out var subscription)) events.Add(subscription);
        }

        if (events.Count == 0) return null;

        var location = typeSymbol.Locations.FirstOrDefault(static location => location.IsInSource);
        string sortPath = location?.SourceTree?.FilePath ?? string.Empty;
        int sortStart = location?.SourceSpan.Start ?? 0;

        return new ServiceInfo(
            typeSymbol.ToDisplayString(TypeDisplayFormat),
            command,
            requestType,
            encryptType,
            disableLog,
            events.ToImmutable(),
            sortPath,
            sortStart
        );
    }

    private static bool TryGetEventSubscription(AttributeData attribute, out EventSubscriptionInfo subscription)
    {
        subscription = default;

        if (attribute.AttributeClass is not { Name: EventSubscribeAttributeName } attributeClass ||  attributeClass.ContainingNamespace.ToDisplayString() != EventSubscribeAttributeNamespace)  return false;

        ITypeSymbol? eventType;
        int protocolArgumentIndex;
        if (attributeClass.Arity == 1)
        {
            eventType = attributeClass.TypeArguments[0];
            protocolArgumentIndex = 0;
        }
        else
        {
            if (attribute.ConstructorArguments.Length < 2) return false;

            eventType = attribute.ConstructorArguments[0].Value as ITypeSymbol;
            protocolArgumentIndex = 1;
        }

        if (eventType is null || attribute.ConstructorArguments.Length <= protocolArgumentIndex)  return false;

        subscription = new EventSubscriptionInfo(eventType.ToDisplayString(TypeDisplayFormat), ToInt64(attribute.ConstructorArguments[protocolArgumentIndex].Value, 0));
        return true;
    }

    private static void Output(SourceProductionContext context, ImmutableArray<ServiceInfo> services)
    {
        var orderedServices = services
            .OrderBy(static service => service.SortPath, StringComparer.Ordinal)
            .ThenBy(static service => service.SortStart)
            .ThenBy(static service => service.ServiceType)
            .ToList();

        var source = new StringBuilder();
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("using System.Collections.Frozen;");
        source.AppendLine();
        source.AppendLine("namespace Lagrange.Core.Internal.Context;");
        source.AppendLine();
        source.AppendLine("internal static class ServiceRegistry");
        source.AppendLine("{");
        source.AppendLine("    internal static (global::System.Collections.Frozen.FrozenDictionary<string, global::Lagrange.Core.Services.IService> Services, global::System.Collections.Frozen.FrozenDictionary<global::System.Type, (global::Lagrange.Core.Services.ServiceAttribute Attribute, global::Lagrange.Core.Services.IService Instance)> ServicesEventType) Create(global::Lagrange.Core.Common.Protocols protocol, global::System.Collections.Generic.HashSet<string> disabledLog)");
        source.AppendLine("    {");
        source.AppendLine("        var services = new global::System.Collections.Generic.Dictionary<string, global::Lagrange.Core.Services.IService>();");
        source.AppendLine("        var servicesEventType = new global::System.Collections.Generic.Dictionary<global::System.Type, (global::Lagrange.Core.Services.ServiceAttribute Attribute, global::Lagrange.Core.Services.IService Instance)>();");
        source.AppendLine();

        for (int i = 0; i < orderedServices.Count; i++)
        {
            source.Append("        Register").Append(i).AppendLine("(protocol, disabledLog, services, servicesEventType);");
        }

        source.AppendLine();
        source.AppendLine("        return (services.ToFrozenDictionary(), servicesEventType.ToFrozenDictionary());");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static void AddEvent(global::System.Type eventType, global::Lagrange.Core.Services.ServiceAttribute attribute, global::Lagrange.Core.Services.IService service, global::System.Collections.Generic.Dictionary<global::System.Type, (global::Lagrange.Core.Services.ServiceAttribute Attribute, global::Lagrange.Core.Services.IService Instance)> servicesEventType)");
        source.AppendLine("    {");
        source.AppendLine("        if (servicesEventType.ContainsKey(eventType))");
        source.AppendLine("        {");
        source.AppendLine("            throw new global::Lagrange.Core.Exceptions.ServiceRegistrationException($\"Multiple protocol services are registered for event type '{eventType}'.\");");
        source.AppendLine("        }");
        source.AppendLine();
        source.AppendLine("        servicesEventType[eventType] = (attribute, service);");
        source.AppendLine("    }");

        for (int i = 0; i < orderedServices.Count; i++)
        {
            AppendRegisterMethod(source, i, orderedServices[i]);
        }

        source.AppendLine("}");

        context.AddSource("Lagrange.Core.Internal.Context.ServiceRegistry.g.cs", source.ToString());
    }

    private static void AppendRegisterMethod(StringBuilder source, int index, ServiceInfo service)
    {
        source.AppendLine();
        source.Append("    private static void Register").Append(index).AppendLine("(global::Lagrange.Core.Common.Protocols protocol, global::System.Collections.Generic.HashSet<string> disabledLog, global::System.Collections.Generic.Dictionary<string, global::Lagrange.Core.Services.IService> services, global::System.Collections.Generic.Dictionary<global::System.Type, (global::Lagrange.Core.Services.ServiceAttribute Attribute, global::Lagrange.Core.Services.IService Instance)> servicesEventType)");
        source.AppendLine("    {");
        source.AppendLine("        global::Lagrange.Core.Services.IService? service = null;");

        foreach (var subscription in service.Subscriptions)
        {
            source.Append("        if ((~(global::Lagrange.Core.Common.Protocols)").Append(subscription.Protocol).AppendLine(" & protocol) == global::Lagrange.Core.Common.Protocols.None)");
            source.AppendLine("        {");
            source.Append("            var attribute = new global::Lagrange.Core.Services.ServiceAttribute(")
                .Append(ToLiteral(service.Command))
                .Append(", (global::Lagrange.Core.Common.Entity.RequestType)")
                .Append(service.RequestType)
                .Append(", (global::Lagrange.Core.Common.Entity.EncryptType)")
                .Append(service.EncryptType)
                .Append(')');
            if (service.DisableLog)
            {
                source.Append(" { DisableLog = true }");
            }

            source.AppendLine(";");
            source.AppendLine("            if (service is null)");
            source.AppendLine("            {");
            source.AppendLine("                if (services.ContainsKey(attribute.Command))");
            source.AppendLine("                {");
            source.AppendLine("                    throw new global::Lagrange.Core.Exceptions.ServiceRegistrationException($\"Multiple protocol services are registered for command '{attribute.Command}'.\");");
            source.AppendLine("                }");
            source.AppendLine();
            source.Append("                service = new ").Append(service.ServiceType).AppendLine("();");
            source.AppendLine("                services[attribute.Command] = service;");
            source.AppendLine("                if (attribute.DisableLog) disabledLog.Add(attribute.Command);");
            source.AppendLine("            }");
            source.AppendLine();
            source.Append("            AddEvent(typeof(").Append(subscription.EventType).AppendLine("), attribute, service, servicesEventType);");
            source.AppendLine("        }");
        }

        source.AppendLine("    }");
    }

    private static long ToInt64(object? value, long fallback)
    {
        return value is null ? fallback : Convert.ToInt64(value);
    }

    private static string ToLiteral(string value)
    {
        var builder = new StringBuilder(value.Length + 2);
        builder.Append('"');

        foreach (char c in value)
        {
            switch (c)
            {
                case '\\':
                    builder.Append(@"\\");
                    break;
                case '"':
                    builder.Append("\\\"");
                    break;
                case '\0':
                    builder.Append(@"\0");
                    break;
                case '\a':
                    builder.Append(@"\a");
                    break;
                case '\b':
                    builder.Append(@"\b");
                    break;
                case '\f':
                    builder.Append(@"\f");
                    break;
                case '\n':
                    builder.Append(@"\n");
                    break;
                case '\r':
                    builder.Append(@"\r");
                    break;
                case '\t':
                    builder.Append(@"\t");
                    break;
                case '\v':
                    builder.Append(@"\v");
                    break;
                default:
                    builder.Append(c);
                    break;
            }
        }

        builder.Append('"');
        return builder.ToString();
    }

    private readonly record struct ServiceInfo(
        string ServiceType,
        string Command,
        long RequestType,
        long EncryptType,
        bool DisableLog,
        ImmutableArray<EventSubscriptionInfo> Subscriptions,
        string SortPath,
        int SortStart
    );

    private readonly record struct EventSubscriptionInfo(string EventType, long Protocol);
}
