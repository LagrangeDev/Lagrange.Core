using ProtoBuf;

namespace Lagrange.Core.Core.Packets.System;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class NTPacketUid
{
    [ProtoMember(16)] public string? Uid { get; set; }
}