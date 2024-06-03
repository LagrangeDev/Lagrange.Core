using ProtoBuf;

#pragma warning disable CS8618
// Resharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6_0Response
{
    [ProtoMember(1)] public int RetCode { get; set; }
    
    [ProtoMember(2)] public string RetMsg { get; set; }
    
    [ProtoMember(3)] public string ClientWording { get; set; }
    
    [ProtoMember(4)] public string UploadIp { get; set; }
    
    [ProtoMember(5)] public string ServerDns { get; set; }
    
    [ProtoMember(6)] public int BusId { get; set; }
    
    [ProtoMember(7)] public string FileId { get; set; }
    
    [ProtoMember(8)] public byte[] CheckKey { get; set; }
    
    [ProtoMember(9)] public byte[] FileKey { get; set; }
    
    [ProtoMember(10)] public bool BoolFileExist { get; set; }
    
    [ProtoMember(12)] public List<string> UploadIpLanV4 { get; set; }
    
    [ProtoMember(13)] public List<string> UploadIpLanV6 { get; set; }
    
    [ProtoMember(14)] public uint UploadPort { get; set; }
}