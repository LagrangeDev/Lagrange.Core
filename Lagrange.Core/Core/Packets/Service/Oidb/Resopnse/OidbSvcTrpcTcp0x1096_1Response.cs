using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Resopnse;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x1096_1Response
{
    [ProtoMember(1)] public string? Success { get; set; }
}