using ProtoBuf;

namespace Lagrange.Core.Core.Packets.System;

// ReSharper disable once InconsistentNaming

/// <summary>
/// trpc.qq_new_tech.status_svc.StatusService.SsoHeartBeat
/// </summary>
[ProtoContract]
internal class NTSsoHeartBeat
{
    [ProtoMember(1)] public int Type { get; set; }
}