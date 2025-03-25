using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

/// <summary>
/// This FileUploader should be called manually
/// </summary>
internal static class FileUploader
{
    public static async Task<(int Retcode, string Message)> UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is not FileEntity { FileStream: not null } file) return (-91000, "Not FileEntity");

        var uploadEvent = FileUploadEvent.Create(chain.Uid ?? "", file);
        var result = await context.Business.SendEvent(uploadEvent);
        if (result.Count == 0) return (-90000, "(FileUploadEvent) No Event");
        var uploadResp = (FileUploadEvent)result[0];
        if (!uploadResp.IsSuccess) return (uploadResp.ResultCode, $"(FileUploadEvent) {uploadResp.Message}");

        if (!uploadResp.IsExist)
        {
            var ext = new FileUploadExt
            {
                Unknown1 = 100,
                Unknown2 = 1,
                Entry = new FileUploadEntry
                {
                    BusiBuff = new ExcitingBusiInfo
                    {
                        SenderUin = context.Keystore.Uin,
                    },
                    FileEntry = new ExcitingFileEntry
                    {
                        FileSize = file.FileStream.Length,
                        Md5 = file.FileMd5,
                        CheckKey = file.FileSha1,
                        Md5S2 = file.FileMd5,
                        FileId = uploadResp.FileId,
                        UploadKey = uploadResp.UploadKey
                    },
                    ClientInfo = new ExcitingClientInfo
                    {
                        ClientType = 3,
                        AppId = "100",
                        TerminalType = 3,
                        ClientVer = "1.1.1",
                        Unknown = 4
                    },
                    FileNameInfo = new ExcitingFileNameInfo
                    {
                        FileName = file.FileName
                    },
                    Host = new ExcitingHostConfig
                    {
                        Hosts = new List<ExcitingHostInfo>
                        {
                            new()
                            {
                                Url = new ExcitingUrlInfo
                                {
                                    Unknown = 1,
                                    Host = uploadResp.Ip
                                },
                                Port = uploadResp.Port
                            }
                        }
                    }
                },
                Unknown200 = 1
            };

            file.FileHash = uploadResp.Addon;
            file.FileUuid = uploadResp.FileId;

            bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(95, file.FileStream, await Common.GetTicket(context), file.FileMd5, ext.Serialize().ToArray());
            if (!hwSuccess) return (-91001, "Highway Failed");
        }

        await file.FileStream.DisposeAsync();
        var sendEvent = SendMessageEvent.Create(chain);
        var sendResult = await context.Business.SendEvent(sendEvent);
        if (sendResult.Count == 0) return (-90000, "(SendMessageEvent) No Event");
        var sendResp = (SendMessageEvent)sendResult[0];
        if (sendResp.ResultCode != 0) return (sendResp.ResultCode, $"(SendMessageEvent) Failed!");
        return (0, "ok");
    }

    public static async Task<(int Retcode, string Message)> UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity, string targetDirectory)
    {
        if (entity is not FileEntity { FileStream: not null } file) return (-91000, "Not FileEntity");

        var uploadEvent = GroupFSUploadEvent.Create(chain.GroupUin ?? 0, targetDirectory, file);
        var result = await context.Business.SendEvent(uploadEvent);
        if (result.Count == 0) return (-90000, "(GroupFSUploadEvent) No Event");
        var uploadResp = (GroupFSUploadEvent)result[0];
        if (!uploadResp.IsSuccess) return (uploadResp.ResultCode, $"(GroupFSUploadEvent) {uploadResp.Message}");

        if (!uploadResp.IsExist)
        {
            var ext = new FileUploadExt
            {
                Unknown1 = 100,
                Unknown2 = 1,
                Entry = new FileUploadEntry
                {
                    BusiBuff = new ExcitingBusiInfo
                    {
                        SenderUin = context.Keystore.Uin,
                        ReceiverUin = chain.GroupUin ?? 0,
                        GroupCode = chain.GroupUin ?? 0
                    },
                    FileEntry = new ExcitingFileEntry
                    {
                        FileSize = file.FileStream.Length,
                        Md5 = file.FileMd5,
                        CheckKey = uploadResp.CheckKey,
                        Md5S2 = file.FileMd5,
                        FileId = uploadResp.FileId,
                        UploadKey = uploadResp.UploadKey
                    },
                    ClientInfo = new ExcitingClientInfo
                    {
                        ClientType = 3,
                        AppId = "100",
                        TerminalType = 3,
                        ClientVer = "1.1.1",
                        Unknown = 4
                    },
                    FileNameInfo = new ExcitingFileNameInfo
                    {
                        FileName = file.FileName
                    },
                    Host = new ExcitingHostConfig
                    {
                        Hosts = new List<ExcitingHostInfo>
                        {
                            new()
                            {
                                Url = new ExcitingUrlInfo
                                {
                                    Unknown = 1,
                                    Host = uploadResp.Ip
                                },
                                Port = uploadResp.Port
                            }
                        }
                    }
                }
            };

            bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(71, file.FileStream, await Common.GetTicket(context), file.FileMd5, ext.Serialize().ToArray());
            if (!hwSuccess) return (-91001, "Highway Failed");
        }

        await file.FileStream.DisposeAsync();
        var sendEvent = GroupSendFileEvent.Create(chain.GroupUin ?? 0, uploadResp.FileId);
        var sendResult = await context.Business.SendEvent(sendEvent);
        if (sendResult.Count == 0) return (-90000, "(GroupSendFileEvent) No Event");
        var sendResp = (GroupSendFileEvent)sendResult[0];
        if (!sendResp.IsSuccess) return (sendResp.ResultCode, $"(GroupSendFileEvent) {sendResp.Message}");

        return (0, "ok");
    }
}