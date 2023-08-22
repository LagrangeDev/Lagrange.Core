using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x1253_1Response
{
    [ProtoMember(2)] public string? Success { get; set; }
}