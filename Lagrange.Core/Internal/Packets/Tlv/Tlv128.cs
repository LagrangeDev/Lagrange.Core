using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x128)]
internal class Tlv128 : TlvBody
{
    public Tlv128(BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        Os = appInfo.Os;
        Guid = deviceInfo.Guid.ToByteArray();
    }

    [BinaryProperty] public ushort Const0 { get; set; } = 0;

    [BinaryProperty] public byte GuidNew { get; set; } = 0;

    [BinaryProperty] public byte GuidAvailable { get; set; } = 0;

    [BinaryProperty] public byte GuidChanged { get; set; } = 0;

    [BinaryProperty] public uint GuidFlag { get; set; } = 0;

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string Os { get; set; }

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public byte[] Guid { get; set; }

    [BinaryProperty] public ushort Const1 { get; set; } = 0;
}