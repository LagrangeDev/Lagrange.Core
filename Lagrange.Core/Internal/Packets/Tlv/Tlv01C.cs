using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x01c)]
internal class Tlv01C : TlvBody
{
    [BinaryProperty] public uint ExpireSec { get; set; }
    
    [BinaryProperty] public ushort ExpireMin { get; set; }
}