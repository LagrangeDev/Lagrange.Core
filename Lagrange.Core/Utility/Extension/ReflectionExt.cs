using System.Reflection;

namespace Lagrange.Core.Utility.Extension;

internal static class ReflectionExt
{
    public static List<Type> GetTypeByAttributes<T>(this Assembly assembly, out List<T> attributes) where T : Attribute
    {
        var list = new List<Type>();
        attributes = new List<T>();
        
        foreach (var type in assembly.GetTypes())
        {
            var attribute = type.GetCustomAttribute<T>();
            if (attribute != null)
            {
                list.Add(type);
                attributes.Add(attribute);
            }
        }

        return list;
    }
    
    public static List<Type> GetTypeWithMultipleAttributes<T>(this Assembly assembly, out List<T[]> attributes) where T : Attribute
    {
        var list = new List<Type>();
        attributes = new List<T[]>();
        
        foreach (var type in assembly.GetTypes())
        {
            var attribute = type.GetCustomAttributes<T>().ToArray();
            if (attribute.Length > 0)
            {
                list.Add(type);
                attributes.Add(attribute);
            }
        }

        return list;
    }

    public static List<Type> GetDerivedTypes<T>(this Assembly assembly) where T : class => 
        assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
    
    public static List<Type> GetImplementations<T>(this Assembly assembly) => 
        assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(T))).ToList();
    
    public static MemberInfo[] GetMemberInfoByAttribute<T>(this Type type) where T : Attribute => 
        type.GetMembers().Where(x => x.GetCustomAttribute<T>() != null).ToArray();
    
    public static PropertyInfo[] GetPropertyInfoByAttribute<T>(this Type type) where T : Attribute => 
        type.GetProperties().Where(x => x.GetCustomAttribute<T>() != null).ToArray();
}