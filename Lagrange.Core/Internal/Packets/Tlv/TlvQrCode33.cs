using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[TlvQrCode(0x033)]
internal class TlvQrCode33 : TlvBody
{
    public TlvQrCode33(BotDeviceInfo deviceInfo) => Guid = deviceInfo.Guid.ToByteArray();

    [BinaryProperty(Prefix.None)] public byte[] Guid { get; }
}