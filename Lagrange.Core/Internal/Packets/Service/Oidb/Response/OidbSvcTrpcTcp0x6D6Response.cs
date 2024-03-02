using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x6D6_0Response Upload { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D6_2Response Download { get; set; }
}