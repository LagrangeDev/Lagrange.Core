using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0x644, 1)]
internal class OidbSvcTrpcTcp0x644_1
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x644_1Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x644_1Body
{
    [ProtoMember(1)] public int Field1 { get; set; } // Unknown
    
    [ProtoMember(2)] public int Field2 { get; set; } // Unknown
    
    [ProtoMember(3)] public int Field3 { get; set; } // Unknown
    
    [ProtoMember(4)] public int Field4 { get; set; } // Unknown
}