using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0x10C0, 1)]
internal class OidbSvcTrpcTcp0x10C0_1
{
    [ProtoMember(1)] public int Field1 { get; set; } // Unknown
    
    [ProtoMember(2)] public int Field2 { get; set; } // Unknown
}