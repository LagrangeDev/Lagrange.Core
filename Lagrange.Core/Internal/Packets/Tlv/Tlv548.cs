using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Tencent;

namespace Lagrange.Core.Internal.Packets.Tlv;


[Tlv(0x548)]
internal class Tlv548 : TlvBody
{
    public Tlv548()
    {
        PowValue = ClientPow.GetPow(new PowValue());
    }
    [BinaryProperty(Prefix.None)] public byte[] PowValue { get; set; }
}