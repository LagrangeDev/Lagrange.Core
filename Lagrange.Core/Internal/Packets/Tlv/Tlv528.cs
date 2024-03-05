using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x528)]
internal class Tlv528 : TlvBody
{
    [BinaryProperty(Prefix.None)] public string u1 { get; set; } // {"QIM_invitation_bit":"1"}
}