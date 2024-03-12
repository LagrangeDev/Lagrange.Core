using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.System;
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
    public static async Task<bool> UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is not FileEntity { FileStream: not null } file) return false;
        
        var uploadEvent = FileUploadEvent.Create(chain.Uid ?? "", file);
        var result = await context.Business.SendEvent(uploadEvent);
        var uploadResp = (FileUploadEvent)result[0];
        
        var hwUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResult = await context.Business.SendEvent(hwUrlEvent);
        var ticketResult = (HighwayUrlEvent)highwayUrlResult[0];

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

            bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(95, file.FileStream, ticketResult.SigSession, file.FileMd5, ext.Serialize().ToArray());
            if (!hwSuccess) return false;
        }

        await file.FileStream.DisposeAsync();
        var sendEvent = SendMessageEvent.Create(chain);
        var sendResult = await context.Business.SendEvent(sendEvent);
        return sendResult.Count != 0 && ((SendMessageEvent)sendResult[0]).MsgResult.Result == 0;
    }

    public static async Task<bool> UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity, string targetDirectory)
    {
        if (entity is not FileEntity { FileStream: not null } file) return false;
        
        var uploadEvent = GroupFSUploadEvent.Create(chain.GroupUin ?? 0, targetDirectory, file);
        var result = await context.Business.SendEvent(uploadEvent);
        var uploadResp = (GroupFSUploadEvent)result[0];
        
        var hwUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResult = await context.Business.SendEvent(hwUrlEvent);
        var ticketResult = (HighwayUrlEvent)highwayUrlResult[0];

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

            bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(71, file.FileStream, ticketResult.SigSession, file.FileMd5, ext.Serialize().ToArray());
            if (!hwSuccess) return false;
        }
        
        await file.FileStream.DisposeAsync();
        var sendEvent = GroupSendFileEvent.Create(chain.GroupUin ?? 0, uploadResp.FileId);
        var sendResult = await context.Business.SendEvent(sendEvent);
        return sendResult.Count != 0 && ((GroupSendFileEvent)sendResult[0]).ResultCode == 0;
    }
}