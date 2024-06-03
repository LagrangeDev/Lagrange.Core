using System.Reflection;
using System.Text;
using ProtoBuf;

namespace Lagrange.Core.Test.Utility;

internal static class ProtoGen
{
    public static void GenerateProtoFiles()
    {
        var assembly = typeof(Lagrange.Core.Utility.ServiceInjector).Assembly;
        var types = assembly.GetTypes();
        var sb = new StringBuilder();
        
        sb.AppendLine("syntax = \"proto3\";");
        sb.AppendLine();
        sb.AppendLine("package Lagrange.Core;");
        
        foreach (var type in types)
        {
            if (type.Namespace?.StartsWith("Lagrange.Core.Internal.Packets") != true) continue;

            sb.AppendLine($"message {type.Name} {{");
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                string typeString = ParseType(property.PropertyType);
                sb.AppendLine($"    {GetLastToken(typeString, '.')} {property.Name} = {property.GetCustomAttribute<ProtoMemberAttribute>()?.Tag};");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }
        
        string proto = sb.ToString();
        
        File.WriteAllText("packets.proto", proto);
    }
    
    private static string ParseType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            return $"repeated {ParseType(type.GetGenericArguments()[0])}";
        }
        
        return type.ToString() switch
        {
            "System.UInt64" => "varint",
            "System.UInt32" => "varint",
            "System.UInt16" => "varint",
            "System.Int64" => "varint",
            "System.Int32" => "varint",
            "System.String" => "string",
            "System.Boolean" => "bool",
            "System.Byte[]" => "bytes",
            _ => type.ToString()
        };
    }

    private static string GetLastToken(string str, char separator) => str.Split(separator)[^1];
}