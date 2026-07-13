using Microsoft.CodeAnalysis;

namespace Lagrange.Milky.Generator.Extensions;

public static class SymbolExtension
{
    public static bool DefaultEquals(this ISymbol left, ISymbol? right)
    {
        return left.Equals(right, SymbolEqualityComparer.Default);
    }
}