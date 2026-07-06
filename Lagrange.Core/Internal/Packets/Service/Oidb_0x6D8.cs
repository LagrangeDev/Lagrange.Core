using GroupFileInfo = Lagrange.Core.Internal.Packets.Message.FileInfo;
using GroupFolderInfo = Lagrange.Core.Internal.Packets.Message.FolderInfo;
using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D6D8ReqBody
{
    [ProtoMember(1)] public GetFileInfoReqBody FileInfoReq { get; set; }

    [ProtoMember(2)] public GetFileListReqBody FileListInfoReq { get; set; }

    [ProtoMember(3)] public GetFileCountReqBody GroupFileCntReq { get; set; }

    [ProtoMember(4)] public GetSpaceReqBody GroupSpaceReq { get; set; }

    [ProtoMember(5)] public GetFilePreviewReqBody FilePreviewReq { get; set; }
}

[ProtoPackable]
internal partial class D6D8RspBody
{
    [ProtoMember(1)] public GetFileInfoRspBody FileInfoRsp { get; set; }

    [ProtoMember(2)] public GetFileListRspBody FileListInfoRsp { get; set; }

    [ProtoMember(3)] public GetFileCountRspBody GroupFileCntRsp { get; set; }

    [ProtoMember(4)] public GetSpaceRspBody GroupSpaceRsp { get; set; }

    [ProtoMember(5)] public GetFilePreviewRspBody FilePreviewRsp { get; set; }
}

[ProtoPackable]
internal partial class FileTimeStamp
{
    [ProtoMember(1)] public uint Uint32UploadTime { get; set; }

    [ProtoMember(2)] public string StrFileId { get; set; }
}

[ProtoPackable]
internal partial class GetFileInfoReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public uint Uint32BusId { get; set; }

    [ProtoMember(4)] public string StrFileId { get; set; }

    [ProtoMember(5)] public uint Uint32FieldFlag { get; set; } = 0xFFFFFF;
}

[ProtoPackable]
internal partial class GetFileInfoRspBody
{
    [ProtoMember(1)] public int Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public GroupFileInfo FileInfo { get; set; }
}

[ProtoPackable]
internal partial class GetFileListReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public string StrFolderId { get; set; }

    [ProtoMember(4)] public FileTimeStamp StartTimestamp { get; set; }

    [ProtoMember(5)] public uint Uint32FileCount { get; set; }

    [ProtoMember(6)] public FileTimeStamp MaxTimestamp { get; set; }

    [ProtoMember(7)] public uint Uint32AllFileCount { get; set; }

    [ProtoMember(8)] public uint Uint32ReqFrom { get; set; }

    [ProtoMember(9)] public uint Uint32SortBy { get; set; }

    [ProtoMember(10)] public uint Uint32FilterCode { get; set; }

    [ProtoMember(11)] public ulong Uint64Uin { get; set; }

    [ProtoMember(12)] public uint Uint32FieldFlag { get; set; } = 0xFFFFFF;

    [ProtoMember(13)] public uint Uint32StartIndex { get; set; }

    [ProtoMember(14)] public byte[] BytesContext { get; set; }

    [ProtoMember(15)] public uint Uint32ClientVersion { get; set; }

    [ProtoMember(16)] public uint Uint32WhiteList { get; set; }

    [ProtoMember(17)] public uint Uint32SortOrder { get; set; }

    [ProtoMember(18)] public uint Uint32ShowOnlinedocFolder { get; set; }
}

[ProtoPackable]
internal partial class GetFileListRspBody
{
    [ProtoPackable]
    internal partial class Item
    {
        [ProtoMember(1)] public uint Uint32Type { get; set; }

        [ProtoMember(2)] public GroupFolderInfo FolderInfo { get; set; }

        [ProtoMember(3)] public GroupFileInfo FileInfo { get; set; }
    }

    [ProtoMember(1)] public int Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public bool BoolIsEnd { get; set; }

    [ProtoMember(5)] public List<Item> RptItemList { get; set; }

    [ProtoMember(6)] public FileTimeStamp MsgMaxTimestamp { get; set; }

    [ProtoMember(7)] public uint Uint32AllFileCount { get; set; }

    [ProtoMember(8)] public uint Uint32FilterCode { get; set; }

    [ProtoMember(11)] public bool BoolSafeCheckFlag { get; set; }

    [ProtoMember(12)] public uint Uint32SafeCheckRes { get; set; }

    [ProtoMember(13)] public uint Uint32NextIndex { get; set; }

    [ProtoMember(14)] public byte[] BytesContext { get; set; }

    [ProtoMember(15)] public uint Uint32Role { get; set; }

    [ProtoMember(16)] public uint Uint32OpenFlag { get; set; }
}

[ProtoPackable]
internal partial class GetFileCountReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public uint Uint32BusId { get; set; }
}

[ProtoPackable]
internal partial class GetFileCountRspBody
{
    [ProtoMember(1)] public int Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public uint Uint32AllFileCount { get; set; }

    [ProtoMember(5)] public bool BoolFileTooMany { get; set; }

    [ProtoMember(6)] public uint Uint32LimitCount { get; set; }

    [ProtoMember(7)] public bool BoolIsFull { get; set; }
}

[ProtoPackable]
internal partial class GetSpaceReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }
}

[ProtoPackable]
internal partial class GetSpaceRspBody
{
    [ProtoMember(1)] public int Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public ulong Uint64TotalSpace { get; set; }

    [ProtoMember(5)] public ulong Uint64UsedSpace { get; set; }

    [ProtoMember(6)] public bool BoolAllUpload { get; set; }
}

[ProtoPackable]
internal partial class GetFilePreviewReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public uint Uint32BusId { get; set; }

    [ProtoMember(4)] public string StrFileId { get; set; }
}

[ProtoPackable]
internal partial class GetFilePreviewRspBody
{
    [ProtoMember(1)] public int Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public uint Int32ServerIp { get; set; }

    [ProtoMember(5)] public uint Int32ServerPort { get; set; }

    [ProtoMember(6)] public string StrDownloadDns { get; set; }

    [ProtoMember(7)] public byte[] BytesDownloadUrl { get; set; }

    [ProtoMember(8)] public string StrCookieVal { get; set; }

    [ProtoMember(9)] public byte[] BytesReservedField { get; set; }

    [ProtoMember(10)] public byte[] StrDownloadDnsHttps { get; set; }

    [ProtoMember(11)] public uint Uint32PreviewPortHttps { get; set; }
}
