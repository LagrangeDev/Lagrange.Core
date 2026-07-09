using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lagrange.Core.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class MessageEntitySourceGenerator : IIncrementalGenerator
{
    private const string MessageEntityInterfaceFullName = "Lagrange.Core.Message.Entities.IMessageEntity";

    private static readonly SymbolDisplayFormat TypeDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax { BaseList: not null },
            static (context, _) => ToMessageEntityInfo(context)
        );

        var entities = candidates.SelectMany(static (entity, _) =>
            entity.HasValue ? [entity.GetValueOrDefault()] : ImmutableArray<MessageEntityInfo>.Empty);

        context.RegisterSourceOutput(entities.Collect(), static (context, entities) => Output(context, entities));
    }

    private static MessageEntityInfo? ToMessageEntityInfo(GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol { TypeKind: TypeKind.Class } typeSymbol ||  typeSymbol.IsAbstract)  return null;
        
        var messageEntityInterface = context.SemanticModel.Compilation.GetTypeByMetadataName(MessageEntityInterfaceFullName);
        if (messageEntityInterface is null || !typeSymbol.AllInterfaces.Any(type => SymbolEqualityComparer.Default.Equals(type, messageEntityInterface)))  return null;

        var location = typeSymbol.Locations.FirstOrDefault(static location => location.IsInSource);
        string sortPath = location?.SourceTree?.FilePath ?? string.Empty;
        int sortStart = location?.SourceSpan.Start ?? 0;

        return new MessageEntityInfo(
            typeSymbol.ToDisplayString(TypeDisplayFormat),
            sortPath,
            sortStart
        );
    }

    private static void Output(SourceProductionContext context, ImmutableArray<MessageEntityInfo> entities)
    {
        var orderedEntities = entities
            .OrderBy(static entity => entity.SortPath, StringComparer.Ordinal)
            .ThenBy(static entity => entity.SortStart)
            .ThenBy(static entity => entity.EntityType)
            .ToList();

        var source = new StringBuilder();
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("namespace Lagrange.Core.Message;");
        source.AppendLine();
        source.AppendLine("internal static class MessageEntityRegistry");
        source.AppendLine("{");
        source.AppendLine("    internal static global::System.Collections.Generic.List<global::Lagrange.Core.Message.Entities.IMessageEntity> Create()");
        source.AppendLine("    {");
        source.Append("        var entities = new global::System.Collections.Generic.List<global::Lagrange.Core.Message.Entities.IMessageEntity>(").Append(orderedEntities.Count).AppendLine(");");

        foreach (var entity in orderedEntities)
        {
            source.Append("        entities.Add(new ").Append(entity.EntityType).AppendLine("());");
        }

        source.AppendLine("        return entities;");
        source.AppendLine("    }");
        source.AppendLine("}");

        context.AddSource("Lagrange.Core.Message.MessageEntityRegistry.g.cs", source.ToString());
    }

    private readonly record struct MessageEntityInfo(string EntityType, string SortPath, int SortStart);
}
