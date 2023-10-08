using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x6d6, 5)]
internal class OidbSvcTrpcTcp0x6D6_5
{
    [ProtoMember(6)] public OidbSvcTrpcTcp0x6D6_5Move? Move { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6_5Move
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
    
    [ProtoMember(3)] public uint BusId { get; set; } // 102
    
    [ProtoMember(4)] public string fileId { get; set; }
    
    [ProtoMember(5)] public string ParentDirectory { get; set; }
    
    [ProtoMember(6)] public string TargetDirectory { get; set; }
}