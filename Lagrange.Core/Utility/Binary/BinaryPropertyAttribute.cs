namespace Lagrange.Core.Utility.Binary;

[AttributeUsage(AttributeTargets.Property)]
internal class BinaryPropertyAttribute : Attribute
{
    public Type? Type { get; }
    
    public BinaryPacket.Prefix? Prefix { get; }
    
    public BinaryPropertyAttribute(Type type) => Type = type;
    
    public BinaryPropertyAttribute(BinaryPacket.Prefix prefix) => Prefix = prefix;

    public BinaryPropertyAttribute()
    {
        Type = null;
        Prefix = null;
    }
}