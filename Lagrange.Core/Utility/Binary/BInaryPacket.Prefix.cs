namespace Lagrange.Core.Utility.Binary;

internal partial class BinaryPacket
{
    [Flags]
    public enum Prefix
    {
        None = 0,
        Uint8 = 0b0001,
        Uint16 = 0b0010,
        Uint32 = 0b0100,
        LengthOnly = 0,
        WithPrefix = 0b1000,
    }
}