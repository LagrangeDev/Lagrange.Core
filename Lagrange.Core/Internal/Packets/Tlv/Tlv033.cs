using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x033)]
internal class Tlv033 : TlvBody
{
    public Tlv033(BotDeviceInfo deviceInfo) => Guid = deviceInfo.System.Guid.ToByteArray();

    [BinaryProperty(Prefix.None)] public byte[] Guid { get; }
}