using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// Resharper Disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0x10C0_1Response
{
    [ProtoMember(1)] public List<OidbSvcTrpcTcp0x10C0_1ResponseRequests> Requests { get; set; }
    
    [ProtoMember(2)] public ulong Field2 { get; set; }
    
    [ProtoMember(3)] public ulong NewLatestSeq { get; set; }
    
    [ProtoMember(4)] public uint Field4 { get; set; }
    
    [ProtoMember(5)] public ulong Field5 { get; set; }
    
    [ProtoMember(6)] public uint Field6 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x10C0_1ResponseRequests
{
    [ProtoMember(1)] public ulong Sequence { get; set; }
    
    [ProtoMember(2)] public uint EventType { get; set; } // 13 for exit group, 22 for group request
    
    [ProtoMember(3)] public uint State { get; set; } // 2 for Join, 1 for waiting for action
    
    [ProtoMember(4)] public OidbSvcTrpcTcp0x10C0_1ResponseGroup Group { get; set; }
    
    [ProtoMember(5)] public OidbSvcTrpcTcp0x10C0_1ResponseUser Target { get; set; }
    
    [ProtoMember(6)] public OidbSvcTrpcTcp0x10C0_1ResponseUser? Invitor { get; set; }
    
    [ProtoMember(7)] public OidbSvcTrpcTcp0x10C0_1ResponseUser? Operator { get; set; }
    
    [ProtoMember(9)] public string Field9 { get; set; }
    
    [ProtoMember(10)] public string Comment { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x10C0_1ResponseGroup
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public string GroupName { get; set; } // 13 for exit group, 22 for group request
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x10C0_1ResponseUser
{
    [ProtoMember(1)] public string Uid { get; set; }
    
    [ProtoMember(2)] public string Name { get; set; } // 13 for exit group, 22 for group request
}