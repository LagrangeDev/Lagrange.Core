using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x008)]
internal class Tlv008 : TlvBody
{
    [BinaryProperty] public ushort Const1 { get; set; } = 0x0000;
    [BinaryProperty] public uint u1 { get; set; } = 0x00000804;
    [BinaryProperty] public ushort Const2 { get; set; } = 0x0000;
}