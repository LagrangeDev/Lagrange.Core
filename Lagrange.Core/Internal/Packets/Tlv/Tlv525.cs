using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x525)]
internal class Tlv525 : TlvBody
{
    public Tlv525()
    {
        /*        LoginHistory[] loginHistories = ...;
                LoginHistoryCount = loginHistories.Length;
                var body = new BinaryPacket();
                foreach (var loginHistory in loginHistories)
                {
                    body.WriteLong(loginHistory.Uin, false)
                        .WriteByte(4)
                        .WriteIp(loginHistory.LoginIp)
                        .WriteInt(loginHistory.LoginTime, false)
                        .WriteInt(loginHistory.SubAppId, false);
                }
                LoginHistoryBody = body.ToArray();*/
        LoginHistoryCount = 0;
        LoginHistoryBody = new byte[0];
    }
    [BinaryProperty] public uint u1 { get; set; } = 0x00010536;
    [BinaryProperty] public byte u2 { get; set; } = 0x00;
    [BinaryProperty] public byte u3 { get; set; } = 0x65; // maybe 0x41?
    [BinaryProperty] public byte u4 { get; set; } = 0x01;
    [BinaryProperty] public byte LoginHistoryCount { get; set; }
    [BinaryProperty(Prefix.None)] public byte[] LoginHistoryBody { get; set; }
}