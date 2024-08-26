using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x018)]
internal class Tlv18 : TlvBody
{
    public Tlv18(BotKeystore keystore) => Uin = keystore.Uin;

    [BinaryProperty] public ushort PingVersion { get; set; } = 0;

    [BinaryProperty] public uint SsoVersion { get; set; } = 0x00000005;

    [BinaryProperty] public uint AppId { get; set; } = 0; // const 0, not sure what this is

    [BinaryProperty] public uint AppClientVersion { get; set; } = 8001;

    [BinaryProperty] public uint Uin { get; set; }

    [BinaryProperty] public ushort Field0 { get; set; } = 0;

    [BinaryProperty] public ushort Field1 { get; set; } = 0;
}