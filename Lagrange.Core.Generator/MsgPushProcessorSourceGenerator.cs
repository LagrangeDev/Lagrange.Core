using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Lagrange.Core.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class MsgPushProcessorSourceGenerator : IIncrementalGenerator
{
    private const string MsgPushProcessorAttributeFullName = "Lagrange.Core.Internal.Logic.MsgPushProcessorAttribute";
    private const string MsgPushProcessorBaseFullName = "Lagrange.Core.Internal.Logic.MsgPushProcessorBase";

    private static readonly SymbolDisplayFormat TypeDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
            MsgPushProcessorAttributeFullName,
            static (_, _) => true,
            static (context, _) => ToMsgPushProcessorInfo(context)
        );
        
        var processors = candidates.SelectMany(static (processor, _) => processor.HasValue ? [processor.GetValueOrDefault()] : ImmutableArray<MsgPushProcessorInfo>.Empty);
        context.RegisterSourceOutput(processors.Collect(), static (context, processors) => Output(context, processors));
    }

    private static MsgPushProcessorInfo? ToMsgPushProcessorInfo(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol is not INamedTypeSymbol { TypeKind: TypeKind.Class } typeSymbol || typeSymbol.IsAbstract) return null;

        var processorBase = context.SemanticModel.Compilation.GetTypeByMetadataName(MsgPushProcessorBaseFullName);
        if (processorBase is null || !InheritsFrom(typeSymbol, processorBase))  return null;

        var subscriptions = ImmutableArray.CreateBuilder<MsgPushSubscriptionInfo>();
        foreach (var attribute in context.Attributes)
        {
            if (TryGetSubscription(attribute, out var subscription))
            {
                subscriptions.Add(subscription);
            }
        }

        if (subscriptions.Count == 0) return null;
        
        var location = typeSymbol.Locations.FirstOrDefault(static location => location.IsInSource);
        string sortPath = location?.SourceTree?.FilePath ?? string.Empty;
        int sortStart = location?.SourceSpan.Start ?? 0;

        return new MsgPushProcessorInfo(
            typeSymbol.ToDisplayString(TypeDisplayFormat),
            subscriptions.ToImmutable(),
            sortPath,
            sortStart
        );
    }

    private static bool InheritsFrom(INamedTypeSymbol typeSymbol, INamedTypeSymbol baseType)
    {
        for (var current = typeSymbol.BaseType; current is not null; current = current.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(current, baseType)) return true;
        }

        return false;
    }

    private static bool TryGetSubscription(AttributeData attribute, out MsgPushSubscriptionInfo subscription)
    {
        subscription = default;
        if (attribute.ConstructorArguments.Length == 0) return false;

        long msgType = ToInt64(attribute.ConstructorArguments[0].Value, 0);
        long subType = -1;
        bool requireContent = false;

        if (attribute.ConstructorArguments.Length == 2)
        {
            object? second = attribute.ConstructorArguments[1].Value;
            if (second is bool requireContentValue)
            {
                requireContent = requireContentValue;
            }
            else
            {
                subType = ToInt64(second, -1);
            }
        }
        else if (attribute.ConstructorArguments.Length >= 3)
        {
            subType = ToInt64(attribute.ConstructorArguments[1].Value, -1);
            requireContent = attribute.ConstructorArguments[2].Value is true;
        }

        subscription = new MsgPushSubscriptionInfo(msgType, subType, requireContent);
        return true;
    }

    private static void Output(SourceProductionContext context, ImmutableArray<MsgPushProcessorInfo> processors)
    {
        var orderedProcessors = processors
            .OrderBy(static processor => processor.SortPath, StringComparer.Ordinal)
            .ThenBy(static processor => processor.SortStart)
            .ThenBy(static processor => processor.ProcessorType)
            .ToList();

        var source = new StringBuilder();
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("using System.Collections.Frozen;");
        source.AppendLine();
        source.AppendLine("namespace Lagrange.Core.Internal.Logic;");
        source.AppendLine();
        source.AppendLine("internal static class MsgPushProcessorRegistry");
        source.AppendLine("{");
        source.AppendLine("    internal static global::System.Collections.Frozen.FrozenDictionary<global::Lagrange.Core.Internal.Logic.MsgMatchKey, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.MsgPushProcessorBase>> Create()");
        source.AppendLine("    {");
        source.AppendLine("        var handlers = new global::System.Collections.Generic.Dictionary<global::Lagrange.Core.Internal.Logic.MsgMatchKey, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.MsgPushProcessorBase>>();");
        source.AppendLine();

        for (int i = 0; i < orderedProcessors.Count; i++)
        {
            source.Append("        Register").Append(i).AppendLine("(handlers);");
        }

        source.AppendLine();
        source.AppendLine("        return handlers.ToFrozenDictionary();");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static void AddProcessor(global::Lagrange.Core.Internal.Logic.MsgMatchKey key, global::Lagrange.Core.Internal.Logic.MsgPushProcessorBase processor, global::System.Collections.Generic.Dictionary<global::Lagrange.Core.Internal.Logic.MsgMatchKey, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.MsgPushProcessorBase>> handlers)");
        source.AppendLine("    {");
        source.AppendLine("        if (!handlers.TryGetValue(key, out var list))");
        source.AppendLine("        {");
        source.AppendLine("            list = [];");
        source.AppendLine("            handlers[key] = list;");
        source.AppendLine("        }");
        source.AppendLine();
        source.AppendLine("        list.Add(processor);");
        source.AppendLine("    }");

        for (int i = 0; i < orderedProcessors.Count; i++)
        {
            AppendRegisterMethod(source, i, orderedProcessors[i]);
        }

        source.AppendLine("}");

        context.AddSource("Lagrange.Core.Internal.Logic.MsgPushProcessorRegistry.g.cs", source.ToString());
    }

    private static void AppendRegisterMethod(StringBuilder source, int index, MsgPushProcessorInfo processor)
    {
        source.AppendLine();
        source.Append("    private static void Register").Append(index).AppendLine("(global::System.Collections.Generic.Dictionary<global::Lagrange.Core.Internal.Logic.MsgMatchKey, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.MsgPushProcessorBase>> handlers)");
        source.AppendLine("    {");
        source.Append("        var processor = new ").Append(processor.ProcessorType).AppendLine("();");

        foreach (var subscription in processor.Subscriptions)
        {
            source.Append("        AddProcessor(new global::Lagrange.Core.Internal.Logic.MsgMatchKey((global::Lagrange.Core.Internal.Logic.MsgType)")
                .Append(subscription.MsgType)
                .Append(", ")
                .Append(subscription.SubType)
                .Append(", ")
                .Append(subscription.RequireContent ? "true" : "false")
                .AppendLine("), processor, handlers);");
        }

        source.AppendLine("    }");
    }

    private static long ToInt64(object? value, long fallback)
    {
        return value is null ? fallback : Convert.ToInt64(value);
    }

    private readonly record struct MsgPushProcessorInfo(
        string ProcessorType,
        ImmutableArray<MsgPushSubscriptionInfo> Subscriptions,
        string SortPath,
        int SortStart
    );

    private readonly record struct MsgPushSubscriptionInfo(long MsgType, long SubType, bool RequireContent);
}
