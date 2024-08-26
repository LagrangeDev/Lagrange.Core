using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[TlvQrCode(0x0d1)]
[ProtoContract]
internal class TlvQrCodeD1 : TlvBody
{
    public TlvQrCodeD1(BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        Sys = new NTOS
        {
            Name = deviceInfo.DeviceName,
            OS = appInfo.Os,
        };
        Type = new byte[] { 0x30, 0x01 }; // actually there is a type ext but i'm too lazy to implement it so i just hardcode it
    }

    [ProtoMember(1)] public NTOS Sys { get; set; }

    [ProtoMember(4)] public byte[] Type { get; set; }
}

[TlvQrCode(0x0d1, true)]
[ProtoContract]
internal class TlvQrCodeD1Resp : TlvBody
{
    [ProtoMember(2)] public string Url { get; set; }

    [ProtoMember(3)] public string QrSig { get; set; }
}