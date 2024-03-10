using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Extension;
using System.Text;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x188)]
internal class Tlv188 : TlvBody
{
    public Tlv188(BotDeviceInfo deviceInfo) => AndroidMd5 = Encoding.UTF8.GetBytes(deviceInfo.System.AndroidId).Md5().UnHex();
    [BinaryProperty] public byte[] AndroidMd5 { get; set; }
}