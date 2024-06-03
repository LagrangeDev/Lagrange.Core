using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0x5CF, 11)]
internal class OidbSvcTrpcTcp0x5CF_11
{
    [ProtoMember(1)] public int Field1 { get; set; } // 1
    
    [ProtoMember(3)] public int Field3 { get; set; } // 6
    
    [ProtoMember(4)] public string SelfUid { get; set; }
    
    [ProtoMember(5)] public int Field5 { get; set; } // 0
    
    [ProtoMember(6)] public int Field6 { get; set; } // 80
    
    [ProtoMember(8)] public int Field8 { get; set; } // 2
    
    [ProtoMember(9)] public int Field9 { get; set; } // 0
    
    [ProtoMember(12)] public int Field12 { get; set; } // 1
    
    [ProtoMember(22)] public int Field22 { get; set; } // 1
}