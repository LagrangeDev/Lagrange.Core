namespace Lagrange.Core.Utility.Binary.Tlv;

internal abstract class TlvBody : BinaryPacket
{
    protected TlvBody() { }

    protected TlvBody(byte[] bytes) : base(bytes) { }
}