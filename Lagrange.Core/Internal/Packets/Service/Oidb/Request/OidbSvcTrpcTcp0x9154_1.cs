using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0x9154, 1)]
internal class OidbSvcTrpcTcp0x9154_1
{
    [ProtoMember(1)] public int Field1 { get; set; } // 0
    
    [ProtoMember(2)] public int Field2 { get; set; } // 7
    
    [ProtoMember(3)] public string Field3 { get; set; } // 0
}