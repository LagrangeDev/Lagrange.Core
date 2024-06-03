using System.Reflection;
using Expr = System.Linq.Expressions.Expression;

namespace Lagrange.Core.Utility.Extension;

internal static class ExpressionExt
{
    private static readonly Dictionary<Type, PropertyInfo[]> Properties = new();
    
    private static readonly Dictionary<PropertyInfo, Func<object, object>> Getters = new();
    
    private static readonly Dictionary<PropertyInfo, Action<object, object>> Setters = new();
    
    public static PropertyInfo[] GetPropertiesFromCache(this Type type)
    {
        if (!Properties.TryGetValue(type, out var properties))
        {
            properties = type.GetProperties();
            Properties[type] = properties;
            return properties;
        }

        return properties;
    }

    public static void SetValueByExpr(this PropertyInfo property, object? obj, object val)
    {
        if (obj == null) return;
        
        if (!Setters.TryGetValue(property, out var lambda))
        {
            var expr = Expr.Parameter(typeof(object), "obj");
            var valExpr = Expr.Parameter(typeof(object), "val");

            var castExpr = Expr.Convert(expr, property.DeclaringType ?? throw new InvalidOperationException());
            var castValExpr = Expr.Convert(valExpr, property.PropertyType);

            var propertyExpr = Expr.Property(castExpr, property);
            var assignExpr = Expr.Assign(propertyExpr, castValExpr);
            var lambdaExpr = Expr.Lambda<Action<object, object>>(assignExpr, expr, valExpr);
            
            lambda = lambdaExpr.Compile();
            Setters[property] = lambda;
        }
        
        lambda(obj, val);
    }
    
    public static object? GetValueByExpr(this PropertyInfo property, object? obj)
    {
        if (obj == null) return null;

        if (!Getters.TryGetValue(property, out var lambda))
        {
            var expr = Expr.Parameter(typeof(object), "obj");
            
            var castExpr = Expr.Convert(expr, property.DeclaringType ?? throw new InvalidOperationException());
            var propertyExpr = Expr.Property(castExpr, property);
            var castPropertyExpr = Expr.Convert(propertyExpr, typeof(object));
            
            var lambdaExpr = Expr.Lambda<Func<object, object>>(castPropertyExpr, expr);
            
            lambda = lambdaExpr.Compile();
            Getters[property] = lambda;
        }
        
        return lambda(obj);
    }
}