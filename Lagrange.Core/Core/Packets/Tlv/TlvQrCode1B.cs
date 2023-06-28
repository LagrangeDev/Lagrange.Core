using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Core.Packets.Tlv;

[TlvQrCode(0x01b)]
internal class TlvQrCode1B : TlvBody
{
    [BinaryProperty] public uint Micro { get; set; } = 0;
    
    [BinaryProperty] public uint Version { get; set; } = 0;
    
    [BinaryProperty] public uint Size { get; set; } = 3;
    
    [BinaryProperty] public uint Margin { get; set; } = 4;
    
    [BinaryProperty] public uint Dpi { get; set; } = 72;
    
    [BinaryProperty] public uint EcLevel { get; set; } = 2;
    
    [BinaryProperty] public uint Hint { get; set; } = 2;
    
    [BinaryProperty] public ushort Field0 { get; set; } = 0;
}