using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x142)]
internal class Tlv142 : TlvBody
{
    public Tlv142(BotAppInfo appInfo) => PackageName = appInfo.PackageName;

    [BinaryProperty] public ushort Version { get; set; } = 0;
    
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string PackageName { get; set; }
}