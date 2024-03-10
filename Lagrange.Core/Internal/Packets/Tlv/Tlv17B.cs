using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x17B, true)]
internal class Tlv17B : TlvBody
{
    [BinaryProperty] public ushort u1 { get; set; }

    [BinaryProperty] public ushort u2 { get; set; }
}