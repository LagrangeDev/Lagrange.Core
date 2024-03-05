using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Generator;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x187)]
internal class Tlv187 : TlvBody
{
    public Tlv187()
    {
        Random1 = ByteGen.GenRandomBytes(16);
    }
    [BinaryProperty(Prefix.None)] public byte[] Random1 { get; set; }
}