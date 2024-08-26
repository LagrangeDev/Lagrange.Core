using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class NTOS
{
    [ProtoMember(1)] public string OS { get; set; }

    [ProtoMember(2)] public string Name { get; set; }
}