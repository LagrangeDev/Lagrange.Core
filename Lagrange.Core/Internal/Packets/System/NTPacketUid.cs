using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class NTPacketUid
{
    [ProtoMember(16)] public string? Uid { get; set; }
}