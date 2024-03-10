using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x17E, true)]
internal class Tlv17E : TlvBody
{
    // 你正在一台新设备登录QQ，需进行身份验证
    [BinaryProperty] public string tip { get; set; }
}