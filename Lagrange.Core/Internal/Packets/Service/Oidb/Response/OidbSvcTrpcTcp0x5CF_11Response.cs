using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0x5CF_11Response
{
    [ProtoMember(1)] public uint Field1 { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x5CF_11ResponseInfo Info { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x5CF_11ResponseInfo
{
    [ProtoMember(2)] public uint Field2 { get; set; }
    
    [ProtoMember(3)] public uint Count { get; set; }
    
    [ProtoMember(7)] public List<OidbSvcTrpcTcp0x5CF_11ResponseRequests> Requests { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x5CF_11ResponseRequests
{
    [ProtoMember(1)] public string TargetUid { get; set; }
    
    [ProtoMember(2)] public string SourceUid { get; set; }
    
    [ProtoMember(3)] public uint State { get; set; }
    
    [ProtoMember(4)] public uint Timestamp { get; set; }
    
    [ProtoMember(5)] public string Comment { get; set; }
    
    [ProtoMember(6)] public string Source { get; set; }
    
    [ProtoMember(7)] public uint SourceId { get; set; }
    
    [ProtoMember(8)] public uint SubSourceId { get; set; }
    
    // 剩下的我也猜不出来了 谁猜出来了谁PR吧
}