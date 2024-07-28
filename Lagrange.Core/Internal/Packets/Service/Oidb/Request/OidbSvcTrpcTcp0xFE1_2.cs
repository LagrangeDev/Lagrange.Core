using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// Resharper Disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0xfe1, 2)]
internal class OidbSvcTrpcTcp0xFE1_2
{
    [ProtoMember(1)] public string? Uid { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; }
    
    [ProtoMember(3)] public List<OidbSvcTrpcTcp0xFE1_2Key>? Keys { get; set; } // can be regarded as constants
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2Uin
{
    [ProtoMember(1)] public uint Uin { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; }
    
    [ProtoMember(3)] public List<OidbSvcTrpcTcp0xFE1_2Key>? Keys { get; set; } // can be regarded as constants
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2Key
{
    [ProtoMember(1)] public uint Key { get; set; } // 傻逼
}