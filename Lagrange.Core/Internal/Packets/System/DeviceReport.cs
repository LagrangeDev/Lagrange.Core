using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class DeviceReport
{
    [ProtoMember(1)] public string? AndroidId { get; set; }
    [ProtoMember(2)] public string? Baseband { get; set; }
    [ProtoMember(3)] public string? BootId { get; set; }
    [ProtoMember(4)] public string? Bootloader { get; set; }
    [ProtoMember(5)] public string? Codename { get; set; }
    [ProtoMember(6)] public string? Fingerprint { get; set; }
    [ProtoMember(7)] public string? Incremental { get; set; }
    [ProtoMember(8)] public string? InnerVer { get; set; }
    [ProtoMember(9)] public string? Version { get; set; }
}