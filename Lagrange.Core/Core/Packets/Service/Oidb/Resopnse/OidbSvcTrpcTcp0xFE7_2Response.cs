using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Resopnse;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE7_2Response
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public List<OidbSvcTrpcTcp0xFE7_2Member> Members { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE7_2Member
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xFE7_2Uin Uin { get; set; }
    
    [ProtoMember(10)] public string MemberName { get; set; }
    
    [ProtoMember(11)] public OidbSvcTrpcTcp0xFE7_2Card MemberCard { get; set; }
    
    [ProtoMember(12)] public OidbSvcTrpcTcp0xFE7_2Level? Level { get; set; }

    [ProtoMember(100)] public uint JoinTimestamp { get; set; }
    
    [ProtoMember(101)] public uint LastMsgTimestamp { get; set; }
    
    [ProtoMember(107)] public uint Permission { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE7_2Uin
{
    [ProtoMember(1)] public string Uid { get; set; }
    
    [ProtoMember(4)] public uint Uin { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE7_2Card
{
    [ProtoMember(2)] public string MemberCard { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE7_2Level
{
    [ProtoMember(1)] public List<uint>? Infos { get; set; }
    
    [ProtoMember(2)] public uint Level { get; set; }
}

