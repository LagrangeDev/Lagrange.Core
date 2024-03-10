using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x322)]
internal class Tlv322 : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] DeviceToken { get; set; }
}