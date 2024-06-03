using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Response
{
    [ProtoMember(2)] public List<OidbSvcTrpcTcp0xFE5_2Group> Groups { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Group
{
    [ProtoMember(3)] public uint GroupUin { get; set; }
    
    [ProtoMember(4)] public OidbSvcTrpcTcp0xFE5_2GroupInfo Info { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2GroupInfo
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xFE5_2Member GroupOwner { get; set; }
    
    [ProtoMember(3)] public uint MemberMax { get; set; }
    
    [ProtoMember(4)] public uint MemberCount { get; set; }
    
    [ProtoMember(5)] public string GroupName { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Member
{
    [ProtoMember(2)] public string Uid { get; set; }
}