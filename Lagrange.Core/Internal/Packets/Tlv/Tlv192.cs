using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x192, true)]
internal class Tlv192 : TlvBody
{
    [BinaryProperty(Prefix.None)] public string CaptchaUrl { get; set; }
}