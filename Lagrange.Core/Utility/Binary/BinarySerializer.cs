using System.Reflection;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Utility.Binary;

internal static class BinarySerializer
{
    private static readonly Dictionary<Type, Action<BinaryPacket, object>> SerializeActions = new()
    {
        { typeof(byte), (body, value) => body.WriteByte((byte)value) },
        { typeof(short), (body, value) => body.WriteShort((short)value, false) },
        { typeof(ushort), (body, value) => body.WriteUshort((ushort)value, false) },
        { typeof(int), (body, value) => body.WriteInt((int)value, false) },
        { typeof(uint), (body, value) => body.WriteUint((uint)value, false) },
        { typeof(long), (body, value) => body.WriteLong((long)value, false) },
        { typeof(ulong), (body, value) => body.WriteUlong((ulong)value, false) },
        { typeof(bool), (body, value) => body.WriteBool((bool)value) },
        { typeof(BinaryPacket), (body, value) => body.WritePacket(Serialize((BinaryPacket)value)) }
    };

    private static readonly Dictionary<Type, Action<BinaryPacket, object, BinaryPacket.Prefix>> EnumSerializeActions = new()
    {
        { typeof(string), (body, value, prefix) => body.WriteString((string)value, prefix) },
        { typeof(byte[]), (body, value, prefix) => body.WriteBytes((byte[])value, prefix) },
    };

    private static readonly Dictionary<Type, Func<BinaryPacket, object>> DeserializeActions = new()
    {
        { typeof(byte), body => body.ReadByte() },
        { typeof(short), body => body.ReadShort(false) },
        { typeof(ushort), body => body.ReadUshort(false) },
        { typeof(int), body => body.ReadInt(false) },
        { typeof(uint), body => body.ReadUint(false) },
        { typeof(long), body => body.ReadLong(false) },
        { typeof(ulong), body => body.ReadUlong(false) },
        { typeof(bool), body => body.ReadBool() },
    };
    
    private static readonly Dictionary<Type, Func<BinaryPacket, BinaryPacket.Prefix, object>> EnumDeserializeActions = new()
    {
        { typeof(string), (body, prefix) => body.ReadString(prefix) },
        { typeof(byte[]), (body, prefix) => body.ReadBytes(prefix) },
    };

    public static BinaryPacket Serialize(object obj)
    {
        var type = obj.GetType();
        var body = new BinaryPacket();
        
        foreach (var property in type.GetPropertiesFromCache())
        {
            if (property.GetCustomAttribute<BinaryPropertyAttribute>() == null) continue;
            
            var value = property.GetValueByExpr(obj) ?? throw new InvalidOperationException($"Value is null for {property}");
            if (property.PropertyType == typeof(byte[]) || property.PropertyType == typeof(string))
            {
                var func = EnumSerializeActions[property.PropertyType];
                var prefix = property.GetCustomAttribute<BinaryPropertyAttribute>()?.Prefix ?? BinaryPacket.Prefix.None;
                func(body, value, prefix);
            }
            else
            {
                var func = SerializeActions[property.PropertyType];
                func(body, value);
            }
        }
        
        return body;
    }
    
    public static T Deserialize<T>(this BinaryPacket body)
    {
        var type = typeof(T);
        var obj = type.CreateInstance<T>();

        foreach (var property in type.GetPropertiesFromCache())
        {
            if (property.GetCustomAttribute<BinaryPropertyAttribute>() == null) continue;

            object value;
            if (property.PropertyType == typeof(byte[]) || property.PropertyType == typeof(string))
            {
                var func = EnumDeserializeActions[property.PropertyType];
                var prefix = property.GetCustomAttribute<BinaryPropertyAttribute>()?.Prefix ?? BinaryPacket.Prefix.None;
                value = func(body, prefix);
            }
            else
            {
                var func = DeserializeActions[property.PropertyType];
                value = func(body);
            }
            property.SetValueByExpr(obj, value);
        }
        
        return obj;
    }
    
    public static object Deserialize(this BinaryPacket body, Type type)
    {
        var obj = type.CreateInstance();

        foreach (var property in type.GetPropertiesFromCache())
        {
            if (property.GetCustomAttribute<BinaryPropertyAttribute>() == null) continue;

            object value; 
            if (property.PropertyType == typeof(byte[]) || property.PropertyType == typeof(string))
            {
                var func = EnumDeserializeActions[property.PropertyType];
                var prefix = property.GetCustomAttribute<BinaryPropertyAttribute>()?.Prefix ?? BinaryPacket.Prefix.None;
                value = func(body, prefix);
            }
            else
            {
                var func = DeserializeActions[property.PropertyType];
                value = func(body);
            }
            property.SetValueByExpr(obj, value);
        }
        
        return obj;
    }
}