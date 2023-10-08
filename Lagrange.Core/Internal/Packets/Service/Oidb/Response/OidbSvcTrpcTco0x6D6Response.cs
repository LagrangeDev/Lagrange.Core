using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTco0x6D6Response
{
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D6_2Response Download { get; set; }
}