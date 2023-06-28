namespace Lagrange.Core.Utility.Binary.Tlv.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class TlvEncryptAttribute : Attribute
{
    public TlvEncryptAttribute(KeyType type) => Type = type;

    public KeyType Type { get; }
    
    public enum KeyType
    {
        TgtgtKey,
        Password
    }
}