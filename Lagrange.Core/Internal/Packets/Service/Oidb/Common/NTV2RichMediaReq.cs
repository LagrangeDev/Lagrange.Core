using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Common;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class NTV2RichMediaReq
{
    [ProtoMember(1)] public MultiMediaReqHead ReqHead { get; set; }
    
    [ProtoMember(3)] public DownloadReq Download { get; set; }
}

[ProtoContract]
internal class MultiMediaReqHead
{
    [ProtoMember(1)] public CommonHead Common { get; set; }
    
    [ProtoMember(2)] public SceneInfo Scene { get; set; }

    [ProtoMember(3)] public ClientMeta Client { get; set; }
}

[ProtoContract]
internal class CommonHead
{
    [ProtoMember(1)] public uint RequestId { get; set; } // 1
    
    [ProtoMember(2)] public uint Command { get; set; } // 200
}

[ProtoContract]
internal class SceneInfo
{
    [ProtoMember(101)] public uint RequestType { get; set; } // 1
    
    [ProtoMember(102)] public uint BusinessType { get; set; } // 3
    
    [ProtoMember(200)] public uint SceneType { get; set; } // 1
    
    [ProtoMember(201)] public C2CUserInfo C2C { get; set; }
    
    
    [ProtoMember(202)] public GroupInfo Group { get; set; }
}

[ProtoContract]
internal class C2CUserInfo
{
    [ProtoMember(1)] public uint AccountType { get; set; } // 2
    
    [ProtoMember(2)] public string SelfUid { get; set; }
}


[ProtoContract]
internal class GroupInfo
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
}

[ProtoContract]
internal class ClientMeta
{
    [ProtoMember(1)] public uint AgentType { get; set; } // 2
}

[ProtoContract]
internal class DownloadReq
{
    [ProtoMember(1)] public IndexNode Node { get; set; }
    
    [ProtoMember(2)] public DownloadExt Download { get; set; }
}

[ProtoContract]
internal class IndexNode
{
    [ProtoMember(1)] public FileInfo Info { get; set; }
    
    [ProtoMember(2)] public string FileUuid { get; set; }
    
    [ProtoMember(3)] public uint StoreId { get; set; } // 0旧服务器 1为nt服务器
    
    [ProtoMember(4)] public uint UploadTime { get; set; } // 0

    [ProtoMember(5)] public uint Ttl { get; set; } // 0

    [ProtoMember(6)] public uint SubType { get; set; } // 0
}

[ProtoContract]
internal class FileInfo
{
    [ProtoMember(1)] public uint FileSize { get; set; } // 0
    
    [ProtoMember(2)] public string FileHash { get; set; }
    
    [ProtoMember(3)] public string FileSha1 { get; set; } // ""
    
    [ProtoMember(4)] public string FileName { get; set; }
    
    [ProtoMember(5)] public FileType Type { get; set; }
    
    [ProtoMember(6)] public uint Width { get; set; } // 0
    
    [ProtoMember(7)] public uint Height { get; set; } // 0
    
    [ProtoMember(8)] public uint Time { get; set; } // 2
    
    [ProtoMember(9)] public uint Original { get; set; } // 0
}

[ProtoContract]
internal class FileType
{
    [ProtoMember(1)] public uint Type { get; set; } // 2
    
    [ProtoMember(2)] public uint PicFormat { get; set; } // 0
    
    [ProtoMember(3)] public uint VideoFormat { get; set; } // 0
    
    [ProtoMember(4)] public uint VoiceFormat { get; set; } // 1
}

[ProtoContract]
internal class DownloadExt
{
    [ProtoMember(1)] public PicDownloadExt Pic { get; set; }
    
    [ProtoMember(2)] public VideoDownloadExt Video { get; set; }
    
    [ProtoMember(3)] public PttDownloadExt Ptt { get; set; }
}

[ProtoContract]
internal class VideoDownloadExt
{
    [ProtoMember(1)] public uint BusiType { get; set; } // 0
    
    [ProtoMember(2)] public uint SceneType { get; set; } // 0
    
    [ProtoMember(3)] public uint SubBusiType { get; set; } // 0
}

[ProtoContract]
internal class PicDownloadExt { }

[ProtoContract]
internal class PttDownloadExt { }