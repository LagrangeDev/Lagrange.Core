using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0x5CF, 11)]
internal class OidbSvcTrpcTcp0x5CF_11
{
    [ProtoMember(1)] public int Field1 { get; set; } // Unknown
    
    [ProtoMember(3)] public int Field3 { get; set; } // Unknown
    
    [ProtoMember(4)] public string Field4 { get; set; } // Unknown
    
    [ProtoMember(5)] public int Field5 { get; set; } // Unknown
    
    [ProtoMember(6)] public int Field6 { get; set; } // Unknown
    
    [ProtoMember(8)] public int Field8 { get; set; } // Unknown
    
    [ProtoMember(9)] public int Field9 { get; set; } // Unknown
    
    [ProtoMember(12)] public int Field12 { get; set; } // Unknown
    
    [ProtoMember(22)] public int Field22 { get; set; } // Unknown
}