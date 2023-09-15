using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x107)]
internal class Tlv107 : TlvBody
{
    [BinaryProperty] public ushort PicType { get; set; } = 0x0001;
    
    [BinaryProperty] public byte CapType { get; set; } = 0x0D;
    
    [BinaryProperty] public ushort PicSize { get; set; } = 0x0000;
    
    [BinaryProperty] public byte RetType { get; set; } = 0x01;
}