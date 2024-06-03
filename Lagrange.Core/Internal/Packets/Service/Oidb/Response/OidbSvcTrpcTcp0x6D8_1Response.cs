using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1Response
{
    [ProtoMember(2)] public OidbSvcTrpcTcp0x6D8_1ResponseList? List { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D8_1ResponseCount? Count { get; set; }
    
    [ProtoMember(4)] public OidbSvcTrpcTcp0x6D8_1ResponseSpace? Space { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1ResponseList
{
    [ProtoMember(1)] public int RetCode { get; set; }
    
    [ProtoMember(2)] public string RetMsg { get; set; }
    
    [ProtoMember(3)] public string ClientWording { get; set; }
    
    [ProtoMember(4)] public bool IsEnd { get; set; }
    
    [ProtoMember(5)] public List<OidbSvcTrpcTcp0x6D8_1ResponseItem>? Items { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1ResponseCount
{
    [ProtoMember(4)] public uint FileCount { get; set; }
    
    [ProtoMember(6)] public uint LimitCount { get; set; }
    
    [ProtoMember(7)] public bool IsFull { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1ResponseSpace
{
    [ProtoMember(4)] public ulong TotalSpace { get; set; }
    
    [ProtoMember(5)] public ulong UsedSpace { get; set; }
    
    [ProtoMember(6)] public uint Field6 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1ResponseItem
{
    [ProtoMember(1)] public uint Type { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x6D8_1ResponseFolderInfo FolderInfo { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D8_1ResponseFileInfo FileInfo { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1ResponseFolderInfo
{
    [ProtoMember(1)] public string FolderId { get; set; }
    
    [ProtoMember(2)] public string ParentDirectoryId { get; set; }
    
    [ProtoMember(3)] public string FolderName { get; set; }
    
    [ProtoMember(4)] public uint CreateTime { get; set; }
    
    [ProtoMember(5)] public uint ModifiedTime { get; set; }
    
    [ProtoMember(6)] public uint CreatorUin { get; set; }
    
    [ProtoMember(7)] public string CreatorName { get; set; }
    
    [ProtoMember(8)] public uint TotalFileCount { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8_1ResponseFileInfo
{
    [ProtoMember(1)] public string FileId { get; set; }
    
    [ProtoMember(2)] public string FileName { get; set; }
    
    [ProtoMember(3)] public ulong FileSize { get; set; }
    
    [ProtoMember(4)] public uint BusId { get; set; }
    
    [ProtoMember(5)] public ulong UploadedSize { get; set; }
    
    [ProtoMember(6)] public uint UploadedTime { get; set; }

    [ProtoMember(7)] public uint ExpireTime { get; set; }
    
    [ProtoMember(8)] public uint ModifiedTime { get; set; }
    
    [ProtoMember(9)] public uint DownloadedTimes { get; set; }
    
    [ProtoMember(10)] public byte[] FileSha1 { get; set; }
    
    [ProtoMember(12)] public byte[] FileMd5 { get; set; }
    
    [ProtoMember(14)] public string UploaderName { get; set; }
    
    [ProtoMember(15)] public uint UploaderUin { get; set; }
    
    [ProtoMember(16)] public string ParentDirectory { get; set; }
    
    [ProtoMember(17)] public uint Field17 { get; set; }
    
    [ProtoMember(22)] public string Field22 { get; set; }
}