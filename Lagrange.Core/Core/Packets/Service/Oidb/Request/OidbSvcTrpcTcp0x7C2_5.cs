using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x7c2, 5)]
internal class OidbSvcTrpcTcp0x7C2_5
{
    [ProtoMember(1)] public uint SelfUin { get; set; }
    
    [ProtoMember(2)] public uint TargetUin { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 1
    
    [ProtoMember(4)] public uint Field4 { get; set; } // 1
    
    [ProtoMember(5)] public uint Field5 { get; set; } // 0
    
    [ProtoMember(7)] public string Remark { get; set; } = "";
    
    [ProtoMember(11)] public uint SourceId { get; set; } // 1
    
    [ProtoMember(12)] public uint SubSourceId { get; set; } // 3
    
    [ProtoMember(18)] public string Verify { get; set; } = "";
    
    [ProtoMember(20)] public uint CategoryId { get; set; }
    
    [ProtoMember(26)] public string Answer { get; set; } = "";
    
    [ProtoMember(28)] public uint Field28 { get; set; } // 1

    [ProtoMember(29)] public uint Field29 { get; set; } // 1
}