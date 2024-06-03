namespace Lagrange.Core.Utility.Binary.Tlv.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class TlvQrCodeAttribute : Attribute
{
    public readonly ushort TlvCommand;

    public readonly bool IsResponse;

    public TlvQrCodeAttribute(ushort tlvCommand)
    {
        TlvCommand = tlvCommand;
        IsResponse = false;
    }

    public TlvQrCodeAttribute(ushort tlvCommand, bool isResponse)
    {
        TlvCommand = tlvCommand;
        IsResponse = isResponse;
    }
}
