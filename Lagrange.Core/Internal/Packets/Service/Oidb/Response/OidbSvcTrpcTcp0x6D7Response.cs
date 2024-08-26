using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D7Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x6D7_0Response Create { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x6D7_1_2Response Delete { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D7_1_2Response Rename { get; set; }
}
