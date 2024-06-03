using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6_2Response
{
    [ProtoMember(1)] public int RetCode { get; set; }
    
    [ProtoMember(2)] public string RetMsg { get; set; }
    
    [ProtoMember(3)] public string ClientWording { get; set; }
    
    [ProtoMember(4)] public string DownloadIp { get; set; }

    [ProtoMember(5)] public string DownloadDns { get; set; }

    [ProtoMember(6)] public byte[] DownloadUrl { get; set; }
    
    [ProtoMember(7)] public byte[] FileSha1 { get; set; }
    
    [ProtoMember(8)] public byte[] FileSha3 { get; set; } // ?
    
    [ProtoMember(9)] public byte[] FileMd5 { get; set; }
    
    [ProtoMember(10)] public byte[] CookieVal { get; set; }
    
    [ProtoMember(11)] public string SaveFileName { get; set; }
    
    [ProtoMember(12)] public uint PreviewPort { get; set; }
}