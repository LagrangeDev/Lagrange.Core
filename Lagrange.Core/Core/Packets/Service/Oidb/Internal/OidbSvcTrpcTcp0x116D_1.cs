using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0x116D, 1)]
internal class OidbSvcTrpcTcp0x116D_1
{
    [ProtoMember(1)] public string Uid { get; set; }
    
    [ProtoMember(5)] public int Field5 { get; set; } // Unknown
    
    [ProtoMember(7)] public int Field7 { get; set; } // Unknown
    
    [ProtoMember(100)] public int Field100 { get; set; } // Unknown
    
    [ProtoMember(101)] public int Field101 { get; set; } // Unknown
}