using System.Linq.Expressions;

namespace Lagrange.Core.Utility.Extension;

internal static class TypeExtension
{
    private static readonly Dictionary<Type, Func<object>> CachedExpressions = new();
    
    public static object CreateInstance(this Type type, bool cache = true)
    {
        if (!cache) return Activator.CreateInstance(type) ?? throw new InvalidOperationException();
        if (CachedExpressions.TryGetValue(type, out var instance)) return instance();

        var expression = Expression.New(type);
        var lambda = Expression.Lambda<Func<object>>(expression);
        var func = lambda.Compile();
        
        CachedExpressions[type] = func;
        return func();
    }
    
    public static T CreateInstance<T>(this Type type, bool cache = true) => (T)CreateInstance(type, cache);
}