using System;
using System.Collections.Generic;

namespace Lagrange.Milky.Generator.Utilities;

public class StringLongTupleComparer : IEqualityComparer<(string, long)>
{
    public static StringLongTupleComparer Default { get; } = new();

    public bool Equals((string, long) x, (string, long) y)
    {
        return StringComparer.Ordinal.Equals(x.Item1, y.Item1) && (x.Item2 == y.Item2);
    }

    public int GetHashCode((string, long) obj)
    {
        return HashCode.Combine(obj.Item1, obj.Item2);
    }
}