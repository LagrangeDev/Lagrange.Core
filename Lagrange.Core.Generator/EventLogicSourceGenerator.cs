using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lagrange.Core.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class EventLogicSourceGenerator : IIncrementalGenerator
{
    private const string LogicInterfaceFullName = "Lagrange.Core.Internal.Logic.ILogic";
    private const string EventSubscribeAttributeNamespace = "Lagrange.Core.Internal.Events";
    private const string EventSubscribeAttributeName = "EventSubscribeAttribute";

    private static readonly SymbolDisplayFormat TypeDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    { 
        var candidates = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax { BaseList: not null },
            static (context, _) => ToEventLogicInfo(context)
        );

        var logics = candidates.SelectMany(static (logic, _) =>
            logic.HasValue ? [logic.GetValueOrDefault()] : ImmutableArray<EventLogicInfo>.Empty);

        context.RegisterSourceOutput(logics.Collect(), static (context, logics) => Output(context, logics));
    }

    private static EventLogicInfo? ToEventLogicInfo(GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol { TypeKind: TypeKind.Class } typeSymbol || typeSymbol.IsAbstract)  return null;

        var logicInterface = context.SemanticModel.Compilation.GetTypeByMetadataName(LogicInterfaceFullName);
        if (logicInterface is null || !typeSymbol.AllInterfaces.Any(type => SymbolEqualityComparer.Default.Equals(type, logicInterface)))  return null;

        var events = ImmutableArray.CreateBuilder<EventSubscriptionInfo>();
        foreach (var attribute in typeSymbol.GetAttributes())
        {
            if (TryGetEventSubscription(attribute, out var subscription))
            {
                events.Add(subscription);
            }
        }

        var location = typeSymbol.Locations.FirstOrDefault(static location => location.IsInSource);
        string sortPath = location?.SourceTree?.FilePath ?? string.Empty;
        int sortStart = location?.SourceSpan.Start ?? 0;

        return new EventLogicInfo(
            typeSymbol.ToDisplayString(TypeDisplayFormat),
            events.ToImmutable(),
            sortPath,
            sortStart
        );
    }

    private static bool TryGetEventSubscription(AttributeData attribute, out EventSubscriptionInfo subscription)
    {
        subscription = default;

        if (attribute.AttributeClass is not { Name: EventSubscribeAttributeName } attributeClass || attributeClass.ContainingNamespace.ToDisplayString() != EventSubscribeAttributeNamespace)
        {
            return false;
        }

        ITypeSymbol? eventType;
        if (attributeClass.Arity == 1)
        {
            eventType = attributeClass.TypeArguments[0];
        }
        else
        {
            if (attribute.ConstructorArguments.Length == 0) return false;
            eventType = attribute.ConstructorArguments[0].Value as ITypeSymbol;
        }

        if (eventType is null) return false;

        subscription = new EventSubscriptionInfo(eventType.ToDisplayString(TypeDisplayFormat));
        return true;
    }

    private static void Output(SourceProductionContext context, ImmutableArray<EventLogicInfo> logics)
    {
        var orderedLogics = logics
            .OrderBy(static logic => logic.SortPath, StringComparer.Ordinal)
            .ThenBy(static logic => logic.SortStart)
            .ThenBy(static logic => logic.LogicType)
            .ToList();

        var source = new StringBuilder();
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("using System.Collections.Frozen;");
        source.AppendLine();
        source.AppendLine("namespace Lagrange.Core.Internal.Context;");
        source.AppendLine();
        source.AppendLine("internal static class EventLogicRegistry");
        source.AppendLine("{");
        source.AppendLine("    internal static (global::System.Collections.Frozen.FrozenDictionary<global::System.Type, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.ILogic>> Events, global::System.Collections.Frozen.FrozenDictionary<global::System.Type, global::Lagrange.Core.Internal.Logic.ILogic> Logics) Create(global::Lagrange.Core.BotContext context)");
        source.AppendLine("    {");
        source.AppendLine("        var events = new global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.ILogic>>();");
        source.AppendLine("        var logics = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Lagrange.Core.Internal.Logic.ILogic>();");
        source.AppendLine();

        for (int i = 0; i < orderedLogics.Count; i++)
        {
            source.Append("        Register").Append(i).AppendLine("(context, events, logics);");
        }

        source.AppendLine();
        source.AppendLine("        return (events.ToFrozenDictionary(), logics.ToFrozenDictionary());");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static void AddEvent(global::System.Type eventType, global::Lagrange.Core.Internal.Logic.ILogic logic, global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.ILogic>> events)");
        source.AppendLine("    {");
        source.AppendLine("        if (!events.TryGetValue(eventType, out var list))");
        source.AppendLine("        {");
        source.AppendLine("            list = [];");
        source.AppendLine("            events.Add(eventType, list);");
        source.AppendLine("        }");
        source.AppendLine();
        source.AppendLine("        list.Add(logic);");
        source.AppendLine("    }");

        for (int i = 0; i < orderedLogics.Count; i++)
        {
            AppendRegisterMethod(source, i, orderedLogics[i]);
        }

        source.AppendLine("}");

        context.AddSource("Lagrange.Core.Internal.Context.EventLogicRegistry.g.cs", source.ToString());
    }

    private static void AppendRegisterMethod(StringBuilder source, int index, EventLogicInfo logic)
    {
        source.AppendLine();
        source.Append("    private static void Register").Append(index).AppendLine("(global::Lagrange.Core.BotContext context, global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.List<global::Lagrange.Core.Internal.Logic.ILogic>> events, global::System.Collections.Generic.Dictionary<global::System.Type, global::Lagrange.Core.Internal.Logic.ILogic> logics)");
        source.AppendLine("    {");
        source.Append("        var logic = new ").Append(logic.LogicType).AppendLine("(context);");

        foreach (var subscription in logic.Subscriptions)
        {
            source.Append("        AddEvent(typeof(").Append(subscription.EventType).AppendLine("), logic, events);");
        }

        source.Append("        logics[typeof(").Append(logic.LogicType).AppendLine(")] = logic;");
        source.AppendLine("    }");
    }

    private readonly record struct EventLogicInfo(
        string LogicType,
        ImmutableArray<EventSubscriptionInfo> Subscriptions,
        string SortPath,
        int SortStart
    );

    private readonly record struct EventSubscriptionInfo(string EventType);
}
