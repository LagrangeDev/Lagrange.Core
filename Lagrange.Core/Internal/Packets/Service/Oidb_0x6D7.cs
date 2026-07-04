using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class CreateFolderReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public string StrParentFolderId { get; set; }

    [ProtoMember(4)] public string StrFolderName { get; set; }
}

[ProtoPackable]
internal partial class CreateFolderRspBody
{
    [ProtoMember(1)] public long Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public FolderInfo FolderInfo { get; set; }
}

[ProtoPackable]
internal partial class DeleteFolderReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public string StrFolderId { get; set; }
}

[ProtoPackable]
internal partial class DeleteFolderRspBody
{
    [ProtoMember(1)] public long Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }
}

[ProtoPackable]
internal partial class MoveFolderReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public string StrFolderId { get; set; }

    [ProtoMember(4)] public string StrParentFolderId { get; set; }

    [ProtoMember(5)] public string StrDestFolderId { get; set; }
}

[ProtoPackable]
internal partial class MoveFolderRspBody
{
    [ProtoMember(1)] public long Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public FolderInfo FolderInfo { get; set; }
}

[ProtoPackable]
internal partial class RenameFolderReqBody
{
    [ProtoMember(1)] public ulong Uint64GroupCode { get; set; }

    [ProtoMember(2)] public uint Uint32AppId { get; set; }

    [ProtoMember(3)] public string StrFolderId { get; set; }

    [ProtoMember(4)] public string StrNewFolderName { get; set; }
}

[ProtoPackable]
internal partial class RenameFolderRspBody
{
    [ProtoMember(1)] public long Int32RetCode { get; set; }

    [ProtoMember(2)] public string StrRetMsg { get; set; }

    [ProtoMember(3)] public string StrClientWording { get; set; }

    [ProtoMember(4)] public FolderInfo FolderInfo { get; set; }
}

[ProtoPackable]
internal partial class D6D7ReqBody
{
    [ProtoMember(1)] public CreateFolderReqBody CreateFolderReq { get; set; }

    [ProtoMember(2)] public DeleteFolderReqBody DeleteFolderReq { get; set; }

    [ProtoMember(3)] public RenameFolderReqBody RenameFolderReq { get; set; }

    [ProtoMember(4)] public MoveFolderReqBody MoveFolderReq { get; set; }
}

[ProtoPackable]
internal partial class D6D7RspBody
{
    [ProtoMember(1)] public CreateFolderRspBody CreateFolderRsp { get; set; }

    [ProtoMember(2)] public DeleteFolderRspBody DeleteFolderRsp { get; set; }

    [ProtoMember(3)] public RenameFolderRspBody RenameFolderRsp { get; set; }

    [ProtoMember(4)] public MoveFolderRspBody MoveFolderRsp { get; set; }
}
