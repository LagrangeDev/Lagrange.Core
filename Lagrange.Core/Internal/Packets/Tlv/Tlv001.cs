using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x001)]
internal class Tlv001 : TlvBody
{
    public Tlv001(BotKeystore keystore, bool Isemp)
    {
        Uin = keystore.Uin;
        Time = (uint)DateTimeOffset.Now.ToUnixTimeSeconds();
        u1 = Isemp ? 0x78E4EA00U : 0;
    }
    [BinaryProperty] public ushort IpVer { get; set; } = 1;
    [BinaryProperty] public uint Rand { get; set; } = (uint)Random.Shared.Next();
    [BinaryProperty] public uint Uin { get; set; }
    [BinaryProperty] public uint Time { get; set; }
    [BinaryProperty] public uint u1 { get; set; }
    [BinaryProperty] public ushort Const { get; set; } = 0;
}