using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x145)]
internal class Tlv145 : TlvBody
{
    public Tlv145(BotDeviceInfo deviceInfo) => Guid = deviceInfo.Guid.ToByteArray();

    [BinaryProperty(Prefix.None)] public byte[] Guid { get; set; }
}