using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using System.Net.Sockets;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x52D)]
internal class Tlv52D : TlvBody
{
    public Tlv52D(BotDeviceInfo deviceInfo)
    {
        var deviceReport = new DeviceReport {
            AndroidId = deviceInfo.System.AndroidId,
            Baseband = deviceInfo.Model.BaseBand,
            BootId = deviceInfo.System.BootId,
            Bootloader = deviceInfo.System.BootLoader,
            Codename = deviceInfo.Model.CodeName,
            Fingerprint = deviceInfo.System.FingerPrint,
            Incremental = deviceInfo.System.Incremental,
            InnerVer = deviceInfo.System.InnerVer,
            Version = deviceInfo.System.Version,
        };
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, deviceReport);
        DeviceReport = stream.ToArray();
    }

    [BinaryProperty(Prefix.None)] public byte[] DeviceReport { get; set; }
}