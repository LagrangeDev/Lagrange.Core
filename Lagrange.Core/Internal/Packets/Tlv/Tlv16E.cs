using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x16E)]
internal class Tlv16E : TlvBody
{
    public Tlv16E(BotDeviceInfo deviceInfo) => DeviceName = deviceInfo.DeviceName;

    [BinaryProperty(Prefix.None)] public string DeviceName { get; set; }
}