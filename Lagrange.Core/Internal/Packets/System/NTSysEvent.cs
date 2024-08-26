using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class NTSysEvent
{
    [ProtoMember(1)] public string Ip { get; set; }

    [ProtoMember(2)] public long Sid { get; set; }

    [ProtoMember(3)] public NTSysEventSub Sub { get; set; }
}