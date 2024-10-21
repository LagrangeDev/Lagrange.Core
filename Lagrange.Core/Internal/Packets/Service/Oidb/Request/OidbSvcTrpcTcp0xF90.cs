using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xF90
{
    [ProtoMember(1)] public uint GroupUin { get; set; }

    [ProtoMember(2)] public uint Sequence { get; set; }
}