using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Generator;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x401)]
internal class Tlv401 : TlvBody
{
    public Tlv401(BotDeviceInfo deviceInfo, BotKeystore keystore)
    {
        byte[] tmp = new BinaryPacket().WriteBytes(deviceInfo.System.Guid.ToByteArray(), Prefix.None)
            .WriteBytes(ByteGen.GenRandomBytes(16), Prefix.None)
            .WriteBytes(keystore.Session.T402 ?? Array.Empty<byte>(), Prefix.None)
            .ToArray();
        T401 = tmp.Md5().UnHex();
    }
    [BinaryProperty(Prefix.None)] public byte[] T401 { get; set; }
}