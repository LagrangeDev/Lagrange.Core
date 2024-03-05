using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Extension;
using System.Text;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x109)]
internal class Tlv109 : TlvBody
{
    public Tlv109(BotDeviceInfo deviceInfo) => MD5IMEI = Encoding.UTF8.GetBytes(deviceInfo.Model.Imei).Md5().UnHex();

    [BinaryProperty(Prefix.None)] public byte[] MD5IMEI { get; set; }
}