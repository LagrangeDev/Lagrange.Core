using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x6D6_0Response Upload { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D6_2Response Download { get; set; }
    
    [ProtoMember(4)] public OidbSvcTrpcTcp0x6D6_3_4_5Response Delete { get; set; }
    
    [ProtoMember(5)] public OidbSvcTrpcTcp0x6D6_3_4_5Response Rename { get; set; }
    
    [ProtoMember(6)] public OidbSvcTrpcTcp0x6D6_3_4_5Response Move { get; set; }
}