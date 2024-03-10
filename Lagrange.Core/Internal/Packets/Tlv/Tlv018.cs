using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x018)]
internal class Tlv018 : TlvBody
{
    public Tlv018(BotKeystore keystore, BotAppInfo appInfo) {
        AppId = appInfo.AppId;
        AppClientVersion = appInfo.AppClientVersion;
        Uin = keystore.Uin;
    }

    [BinaryProperty] public ushort PingVersion { get; set; } = 1;

    [BinaryProperty] public uint SsoVersion { get; set; } = 1536;

    [BinaryProperty] public uint AppId { get; set; }

    [BinaryProperty] public uint AppClientVersion { get; set; }
    
    [BinaryProperty] public uint Uin { get; set; }
    
    [BinaryProperty] public ushort Field0 { get; set; } = 0;
    
    [BinaryProperty] public ushort Field1 { get; set; } = 0;
}

[Tlv(0x018, true)]
internal class Tlv018Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] TempPassword { get; set; }
}