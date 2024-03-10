using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x154)]
internal class Tlv154 : TlvBody
{
    public Tlv154(int ssoSeq) => SsoSeq = ssoSeq;
    
    [BinaryProperty] public int SsoSeq { get; set; }
}