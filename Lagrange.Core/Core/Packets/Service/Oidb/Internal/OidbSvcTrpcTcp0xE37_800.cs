using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0xE37, 800)]
public class OidbSvcTrpcTcp0xE37_800
{
    [ProtoMember(1)] public uint SubCommand => 800;
    
    [ProtoMember(2)] public int Field2 { get; set; } // Unknown
    
    [ProtoMember(101)] public int Field101 { get; set; } // Unknown
    
    [ProtoMember(102)] public int Field102 { get; set; } // Unknown
    
    [ProtoMember(200)] public int Field200 { get; set; } // Unknown
}

[ProtoContract]
public class OidbSvcTrpcTcp0xE37_800Body
{
    [ProtoMember(10)] public string SenderUid { get; set; }
    
    [ProtoMember(20)] public string ReceiverUid { get; set; }
    
    [ProtoMember(30)] public string FileUuid { get; set; }
    
    [ProtoMember(40)] public string FileHash { get; set; }
}