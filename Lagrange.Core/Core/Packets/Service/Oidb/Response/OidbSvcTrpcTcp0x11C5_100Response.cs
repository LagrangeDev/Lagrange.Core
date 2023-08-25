using Lagrange.Core.Core.Packets.Message.Element.Implementation;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// Resharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x11C5_100ResponseResult Result { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x11C5_100ResponseData Data { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100ResponseResult
{
    [ProtoMember(3)] public string Success { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100ResponseData
{
    [ProtoMember(1)] public string HighwayTicket { get; set; }
    
    [ProtoMember(2)] public uint Expiration { get; set; } // 3600 看着有点像
    
    [ProtoMember(3)] public List<OidbSvcTrpcTcp0x11C5_100ResponseIP> IPs { get; set; }
    
    [ProtoMember(6)] public byte[] CommonAdditional { get; set; }
    
    [ProtoMember(8)] public NotOnlineImage ImageInfo { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100ResponseIP
{
    [ProtoMember(1)] public uint IPv4Address { get; set; }
    
    [ProtoMember(2)] public uint IPv4Port { get; set; }
    
    [ProtoMember(3)] public uint IPv6Address { get; set; }
    
    [ProtoMember(4)] public uint IPv6Port { get; set; }
    
    [ProtoMember(5)] public uint CountryCode { get; set; }
}