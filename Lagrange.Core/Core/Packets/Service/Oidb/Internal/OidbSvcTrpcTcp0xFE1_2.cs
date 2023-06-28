using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0xFE1, 2)]
public class OidbSvcTrpcTcp0xFE1_2
{
    [ProtoMember(1)] public string Uid { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0xFE1_2Body Body { get; set; }
}

[ProtoContract]
public class OidbSvcTrpcTcp0xFE1_2Body
{
    [ProtoMember(1)] public List<int> Friends { get; set; }
}