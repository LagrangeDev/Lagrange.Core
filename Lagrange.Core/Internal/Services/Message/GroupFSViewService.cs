using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<GroupFSListEventReq>(Protocols.All)]
[EventSubscribe<GroupFSCountEventReq>(Protocols.All)]
[EventSubscribe<GroupFSSpaceEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x6d8_1")]
internal class GroupFSViewService : BaseService<GroupFSViewEventReq, GroupFSViewEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(GroupFSViewEventReq input, BotContext context)
    {
        var (service, body) = input switch
        {
            GroupFSListEventReq list => (1u, new D6D8ReqBody
            {
                FileListInfoReq = new GetFileListReqBody
                {
                    Uint64GroupCode = (ulong)list.GroupUin,
                    Uint32AppId = 7,
                    StrFolderId = list.TargetDirectory,
                    Uint32FileCount = list.FileCount,
                    Uint32SortBy = 1,
                    Uint32StartIndex = list.StartIndex,
                    Uint32SortOrder = 2,
                    Uint32ShowOnlinedocFolder = 0
                }
            }),
            GroupFSCountEventReq count => (2u, new D6D8ReqBody
            {
                GroupFileCntReq = new GetFileCountReqBody
                {
                    Uint64GroupCode = (ulong)count.GroupUin,
                    Uint32AppId = 7,
                    Uint32BusId = 6
                }
            }),
            GroupFSSpaceEventReq space => (3u, new D6D8ReqBody
            {
                GroupSpaceReq = new GetSpaceReqBody
                {
                    Uint64GroupCode = (ulong)space.GroupUin,
                    Uint32AppId = 7
                }
            }),
            _ => throw new InvalidOperationException($"Unsupported group file-system view event: {input.GetType().Name}")
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(new Oidb
        {
            Command = 0x6d8,
            Service = service,
            Body = ProtoHelper.Serialize(body),
            Reserved = 1
        }));
    }

    protected override ValueTask<GroupFSViewEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var oidb = ProtoHelper.Deserialize<Oidb>(input.Span);
        if (oidb.Result != 0) throw new OperationException((int)oidb.Result, oidb.Message);

        var response = ProtoHelper.Deserialize<D6D8RspBody>(oidb.Body.Span);

        if (response.FileListInfoRsp is { } list)
        {
            var entries = (list.RptItemList ?? []).Select<GetFileListRspBody.Item, IBotFSEntry>(item => item.Uint32Type switch
            {
                1 => CreateFileEntry(item.FileInfo),
                2 => CreateFolderEntry(item.FolderInfo),
                _ => throw new InvalidOperationException($"Unrecognized group file-system entry type: {item.Uint32Type}")
            }).ToList();

            return ValueTask.FromResult<GroupFSViewEventResp>(
                new GroupFSListEventResp(list.Int32RetCode, list.StrRetMsg, entries, list.BoolIsEnd)
            );
        }

        if (response.GroupFileCntRsp is { } count)
        {
            return ValueTask.FromResult<GroupFSViewEventResp>(
                new GroupFSCountEventResp(count.Int32RetCode, count.StrRetMsg, count.Uint32AllFileCount, count.Uint32LimitCount, count.BoolIsFull)
            );
        }

        if (response.GroupSpaceRsp is { } space)
        {
            return ValueTask.FromResult<GroupFSViewEventResp>(
                new GroupFSSpaceEventResp(space.Int32RetCode, space.StrRetMsg, space.Uint64TotalSpace, space.Uint64UsedSpace)
            );
        }

        throw new InvalidOperationException($"Invalid packet is given to {nameof(GroupFSViewService)}");
    }

    private static BotFileEntry CreateFileEntry(Lagrange.Core.Internal.Packets.Message.FileInfo file)
    {
        return new BotFileEntry(
            file.FileId,
            file.FileName,
            file.ParentFolderId,
            file.FileSize,
            FromUnixTime(file.DeadTime),
            FromUnixTime(file.ModifyTime),
            (long)file.UploaderUin,
            FromUnixTime(file.UploadTime),
            file.DownloadTimes
        );
    }

    private static BotFolderEntry CreateFolderEntry(Lagrange.Core.Internal.Packets.Message.FolderInfo folder)
    {
        return new BotFolderEntry(
            folder.FolderId,
            folder.ParentFolderId,
            folder.FolderName,
            FromUnixTime(folder.CreateTime),
            FromUnixTime(folder.ModifyTime),
            (long)folder.CreateUin,
            folder.TotalFileCount
        );
    }

    private static DateTime FromUnixTime(uint seconds) => DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
}
