using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x0d1)]
[ProtoContract]
internal class Tlv0D1 : TlvBody
{
    public Tlv0D1(BotAppInfo appInfo, BotDeviceInfo deviceInfo)
    {
        Sys = new NTOS
        {
            Name = deviceInfo.Model.DeviceName,
            OS = appInfo.Os,
        };
        Type = new byte[] { 0x30, 0x01 }; // actually there is a type ext but i'm too lazy to implement it so i just hardcode it
    }
    
    [ProtoMember(1)] public NTOS Sys { get; set; }
    
    [ProtoMember(4)] public byte[] Type { get; set; }
}

[Tlv(0x0D1, true)]
[ProtoContract]
internal class Tlv0D1Response : TlvBody
{
    [ProtoMember(2)] public string Url { get; set; }
    
    [ProtoMember(3)] public string QrSig { get; set; }
}