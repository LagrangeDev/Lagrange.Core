using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class SsoInfoSyncResponse
{
    [ProtoMember(3)] public uint Field3 { get; set; }

    [ProtoMember(4)] public uint Field4 { get; set; }

    [ProtoMember(6)] public uint Field6 { get; set; }

    [ProtoMember(7)] public RegisterInfoResponse? RegisterInfoResponse { get; set; }

    [ProtoMember(9)] public uint Field9 { get; set; }
}