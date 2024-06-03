using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x7c1, 1)]
internal class OidbSvcTrpcTcp0x7C1_1
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 1
    
    [ProtoMember(2)] public uint SelfUin { get; set; }
    
    [ProtoMember(3)] public uint TargetUin { get; set; }
    
    [ProtoMember(4)] public uint Field4 { get; set; } // 3999
    
    [ProtoMember(5)] public uint Field5 { get; set; } // 0
}