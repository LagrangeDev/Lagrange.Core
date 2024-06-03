using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37Response 
{
    [ProtoMember(1)] public uint Command { get; set; } // 1700
    
    [ProtoMember(2)] public int Seq { get; set; } // 0
    
    [ProtoMember(19)] public ApplyUploadRespV3 Upload { get; set; }
    
    [ProtoMember(101)] public int BusinessId { get; set; } // 3
    
    [ProtoMember(102)] public int ClientType { get; set; } // 1
    
    [ProtoMember(200)] public int FlagSupportMediaPlatform { get; set; } // 1
}

[ProtoContract]
internal class ApplyUploadRespV3
{
    [ProtoMember(10)] public int RetCode { get; set; }
    
    [ProtoMember(20)] public string RetMsg { get; set; }
    
    [ProtoMember(30)] public long TotalSpace { get; set; }
    
    [ProtoMember(40)] public long UsedSpace { get; set; }
    
    [ProtoMember(50)] public long UploadedSize { get; set; }
    
    [ProtoMember(60)] public string UploadIp { get; set; }
    
    [ProtoMember(70)] public string UploadDomain { get; set; }
    
    [ProtoMember(80)] public uint UploadPort { get; set; }
    
    [ProtoMember(90)] public string Uuid { get; set; }
    
    [ProtoMember(100)] public byte[] UploadKey { get; set; }
    
    [ProtoMember(110)] public bool BoolFileExist { get; set; }
    
    [ProtoMember(120)] public int PackSize { get; set; }
    
    [ProtoMember(130)] public List<string> UploadIpList { get; set; }
    
    [ProtoMember(140)] public int UploadHttpsPort { get; set; }
    
    [ProtoMember(150)] public string UploadHttpsDomain { get; set; }
    
    [ProtoMember(160)] public string UploadDns { get; set; }
    
    [ProtoMember(170)] public string UploadLanip { get; set; }
    
    [ProtoMember(200)] public string FileAddon { get; set; }
    
    [ProtoMember(220)] public byte[] MediaPlatformUploadKey { get; set; }
}