namespace Lagrange.Core.Utility.Binary.Tlv.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class TlvAttribute : Attribute
{
    public readonly ushort TlvCommand;

    public readonly bool IsResponse;

    public TlvAttribute(ushort tlvCommand)
    {
        TlvCommand = tlvCommand;
        IsResponse = false;
    }

    public TlvAttribute(ushort tlvCommand, bool isResponse)
    {
        TlvCommand = tlvCommand;
        IsResponse = isResponse;
    }
}