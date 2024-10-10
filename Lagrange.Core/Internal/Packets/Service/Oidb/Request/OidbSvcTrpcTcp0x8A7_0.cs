using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0x8A7, 0)]
internal class OidbSvcTrpcTcp0x8A7_0
{
    [ProtoMember(1)] public uint SubCmd { get; set; }
    
    [ProtoMember(2)] public uint LimitIntervalTypeForUin { get; set; }
    
    [ProtoMember(3)] public uint LimitIntervalTypeForGroup { get; set; }
    
    [ProtoMember(4)] public uint Uin { get; set; }
    
    [ProtoMember(5)] public uint GroupCode { get; set; }
}