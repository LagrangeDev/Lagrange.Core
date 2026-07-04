using System.Buffers;
using System.Security.Cryptography;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Internal.Services;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entities;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Cryptography;
using Lagrange.Core.Utility.Extension;
using Lagrange.Proto.Primitives;
using Lagrange.Proto.Serialization;
using FileInfo = Lagrange.Core.Internal.Packets.Service.FileInfo;

namespace Lagrange.Core.Internal.Logic;

internal class OperationLogic(BotContext context) : ILogic
{
    private const string Tag = nameof(OperationLogic);

    public async Task<Dictionary<string, string>> FetchCookies(List<string> domains) => (await context.EventContext.SendEvent<FetchCookiesEventResp>(new FetchCookiesEventReq(domains))).Cookies;

    public ValueTask<BotSsoPacket> SendPacket(BotSsoPacket packet, RequestType requestType, EncryptType encryptType)
    {
        int sequence = packet.Sequence != 0 ? packet.Sequence : context.ServiceContext.GetNewSequence();
        packet.Sequence = sequence;
        return context.PacketContext.SendPacket(packet, new ServiceAttribute(packet.Command, requestType, encryptType));
    }

    public async Task<(string, uint)> FetchClientKey()
    {
        var result = await context.EventContext.SendEvent<FetchClientKeyEventResp>(new FetchClientKeyEventReq());
        return (result.ClientKey, result.Expiration);
    }

    public async Task<bool> SendNudge(bool isGroup, long peerUin, long targetUin)
    {
        await context.EventContext.SendEvent<NudgeEventResp>(new NudgeEventReq(isGroup, peerUin, targetUin));
        return true;
    }

    public async Task GroupRename(long groupUin, string name)
    {
        await context.EventContext.SendEvent<GroupRenameEventResp>(new GroupRenameEventReq(groupUin, name));
    }

    public async Task GroupSetSpecialTitle(long groupUin, long targetUin, string title)
    {
        if (context.CacheContext.ResolveCachedUid(targetUin) is not { } uid)
        {
            await context.CacheContext.GetMemberList(groupUin, true);
            uid = context.CacheContext.ResolveCachedUid(targetUin) ?? throw new InvalidTargetException(targetUin);
        }
        await context.EventContext.SendEvent<GroupSetSpecialTitleEventResp>(new GroupSetSpecialTitleEventReq(groupUin, uid, title));
    }

    public async Task GroupMemberRename(long groupUin, long targetUin, string name)
    {
        if (context.CacheContext.ResolveCachedUid(targetUin) is not { } uid)
        {
            await context.CacheContext.GetMemberList(groupUin, true);
            uid = context.CacheContext.ResolveCachedUid(targetUin) ?? throw new InvalidTargetException(targetUin);
        }
        await context.EventContext.SendEvent<GroupMemberRenameEventResp>(new GroupMemberRenameEventReq(groupUin, uid, name));
    }

    public async Task GroupQuit(long groupUin)
    {
        await context.EventContext.SendEvent<GroupQuitEventResp>(new GroupQuitEventReq(groupUin));
    }

    public async Task SetGroupTodo(long groupUin, ulong sequence)
    {
        await context.EventContext.SendEvent<GroupSetTodoEventResp>(new GroupSetTodoEventReq(groupUin, sequence));
    }

    public async Task FinishGroupTodo(long groupUin)
    {
        await context.EventContext.SendEvent<GroupFinishTodoEventResp>(new GroupFinishTodoEventReq(groupUin));
    }

    public async Task RemoveGroupTodo(long groupUin)
    {
        await context.EventContext.SendEvent<GroupRemoveTodoEventResp>(new GroupRemoveTodoEventReq(groupUin));
    }

    public async Task<string> GroupFSDownload(long groupUin, string fileId)
    {
        var request = new GroupFSDownloadEventReq(groupUin, fileId);
        var response = await context.EventContext.SendEvent<GroupFSDownloadEventResp>(request);
        return response.FileUrl;
    }

    public async Task GroupFSMove(long groupUin, string fileId, string parentDirectory, string targetDirectory) => await context.EventContext.SendEvent<GroupFSMoveEventResp>(new GroupFSMoveEventReq(groupUin, fileId, parentDirectory, targetDirectory));

    public async Task GroupFSDelete(long groupUin, string fileId) => await context.EventContext.SendEvent<GroupFSDeleteEventResp>(new GroupFSDeleteEventReq(groupUin, fileId));

    public async Task GroupFSCreateFolder(long groupUin, string name, string parentFolderId = "/")
    {
        await context.EventContext.SendEvent<GroupFSCreateFolderEventResp>(new GroupFSCreateFolderEventReq(groupUin, name, parentFolderId));
    }

    public async Task GroupFSDeleteFolder(long groupUin, string folderId)
    {
        await context.EventContext.SendEvent<GroupFSDeleteFolderEventResp>(new GroupFSDeleteFolderEventReq(groupUin, folderId));
    }

    public async Task GroupFSRenameFolder(long groupUin, string folderId, string newFolderName)
    {
        await context.EventContext.SendEvent<GroupFSRenameFolderEventResp>(new GroupFSRenameFolderEventReq(groupUin, folderId, newFolderName));
    }

    public async Task<(ulong, DateTime)> SendFriendFile(long targetUin, Stream fileStream, string? fileName)
    {
        fileName = ResolveFileName(fileStream, fileName);

        var friend = await context.CacheContext.ResolveFriend(targetUin) ?? throw new InvalidTargetException(targetUin);

        var buffer = ArrayPool<byte>.Shared.Rent(10002432);
        int payload = await fileStream.ReadAsync(buffer.AsMemory(0, 10002432));
        var md510m = MD5.HashData(buffer[..payload]);
        ArrayPool<byte>.Shared.Return(buffer);
        fileStream.Seek(0, SeekOrigin.Begin);

        var request = new FileUploadEventReq(friend.Uid, fileStream, fileName, md510m);
        var result = await context.EventContext.SendEvent<FileUploadEventResp>(request);


        if (!result.IsExist)
        {
            var ext = new FileUploadExt
            {
                Unknown1 = 100,
                Unknown2 = 1,
                Entry = new FileUploadEntry
                {
                    BusiBuff = new ExcitingBusiInfo { SenderUin = context.Keystore.Uin },
                    FileEntry = new ExcitingFileEntry
                    {
                        FileSize = fileStream.Length,
                        Md5 = request.FileMd5,
                        CheckKey = request.FileSha1,
                        Md510M = md510m,
                        Sha3 = TriSha1Provider.CalculateTriSha1(fileStream),
                        FileId = result.FileId,
                        UploadKey = result.UploadKey
                    },
                    ClientInfo = new ExcitingClientInfo
                    {
                        ClientType = 3,
                        AppId = "100",
                        TerminalType = 3,
                        ClientVer = "1.1.1",
                        Unknown = 4
                    },
                    FileNameInfo = new ExcitingFileNameInfo { FileName = fileName },
                    Host = new ExcitingHostConfig
                    {
                        Hosts = result.RtpMediaPlatformUploadAddress.Select(x => new ExcitingHostInfo
                        {
                            Url = new ExcitingUrlInfo { Unknown = 1, Host = x.Item1 },
                            Port = x.Item2
                        }).ToList()
                    }
                },
                Unknown200 = 1
            };

            bool success = await context.HighwayContext.UploadFile(fileStream, 95, ProtoHelper.Serialize(ext));
            if (!success) throw new OperationException(-1, "File upload failed");
        }

        ulong sequence = (ulong)Random.Shared.NextInt64(10000, 99999);
        uint random = (uint)Random.Shared.Next();
        var sendResult = await context.EventContext.SendEvent<SendMessageEventResp>(new SendFriendFileEventReq(friend, request, result, sequence, random));
        if (sendResult.Result != 0) throw new OperationException(sendResult.Result);

        return (sequence, DateTimeOffset.FromUnixTimeSeconds(sendResult.SendTime).UtcDateTime);
    }

    private static string ResolveFileName(Stream fileStream, string? fileName)
    {
        if (fileName == null)
        {
            if (fileStream is FileStream file)
            {
                fileName = Path.GetFileName(file.Name);
            }
            else
            {
                Span<byte> bytes = stackalloc byte[16];
                Random.Shared.NextBytes(bytes);
                fileName = Convert.ToHexString(bytes);
            }
        }

        return fileName;
    }

    public async Task<string> SendGroupFile(long groupUin, Stream fileStream, string? fileName, string parentDirectory)
    {
        fileName = ResolveFileName(fileStream, fileName);

        var md5 = fileStream.Md5();
        var request = new GroupFSUploadEventReq(groupUin, fileName, fileStream, parentDirectory, md5);
        var uploadResp = await context.EventContext.SendEvent<GroupFSUploadEventResp>(request);

        var buffer = ArrayPool<byte>.Shared.Rent(10002432);
        int payload = await fileStream.ReadAsync(buffer.AsMemory(0, 10002432));
        var md510m = MD5.HashData(buffer[..payload]);
        ArrayPool<byte>.Shared.Return(buffer);
        fileStream.Seek(0, SeekOrigin.Begin);

        if (!uploadResp.FileExist)
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
                        ReceiverUin = groupUin,
                        GroupCode = groupUin
                    },
                    FileEntry = new ExcitingFileEntry
                    {
                        FileSize = fileStream.Length,
                        Md5 = md5,
                        CheckKey = uploadResp.FileKey,
                        Md510M = md510m,
                        FileId = uploadResp.FileId,
                        UploadKey = uploadResp.CheckKey
                    },
                    ClientInfo = new ExcitingClientInfo
                    {
                        ClientType = 3,
                        AppId = "100",
                        TerminalType = 3,
                        ClientVer = "1.1.1",
                        Unknown = 4
                    },
                    FileNameInfo = new ExcitingFileNameInfo { FileName = fileName },
                    Host = new ExcitingHostConfig
                    {
                        Hosts =
                        [
                            new ExcitingHostInfo
                            {
                                Url = new ExcitingUrlInfo { Unknown = 1, Host = uploadResp.Addr.ip },
                                Port = uploadResp.Addr.uploadPort
                            }
                        ]
                    }
                }
            };

            bool success = await context.HighwayContext.UploadFile(fileStream, 71, ProtoHelper.Serialize(ext));
            if (!success) throw new OperationException(-1, "File upload failed");
        }

        uint random = (uint)Random.Shared.Next();
        var feedResult = await context.EventContext.SendEvent<GroupFileSendEventResp>(new GroupFileSendEventReq(groupUin, uploadResp.FileId, random));
        if (feedResult.RetCode != 0) throw new OperationException(feedResult.RetCode, feedResult.RetMsg);

        return uploadResp.FileId;
    }

    public async Task<BotGroupExtra> FetchGroupExtra(long groupUin)
    {
        var req = new FetchGroupExtraEventReq(groupUin);
        var resp = await context.EventContext.SendEvent<FetchGroupExtraEventResp>(req);
        return resp.Extra;
    }

    public async Task<List<BotGroupNotificationBase>> FetchGroupNotifications(ulong count, ulong start)
    {
        var req = new FetchGroupNotificationsEventReq(count, start);
        var resp = await context.EventContext.SendEvent<FetchGroupNotificationsEventResp>(req);
        return resp.GroupNotifications;
    }

    public async Task<List<BotGroupNotificationBase>> FetchFilteredGroupNotifications(ulong count, ulong start)
    {
        var req = new FetchFilteredGroupNotificationsEventReq(count, start);
        var resp = await context.EventContext.SendEvent<FetchFilteredGroupNotificationsEventResp>(req);
        return resp.GroupNotifications;
    }

    public async Task<BotStranger> FetchStranger(long uid)
    {
        var req = new FetchStrangerByUinEventReq(uid);
        var resp = await context.EventContext.SendEvent<FetchStrangerEventResp>(req);
        return resp.Stranger;
    }

    public async Task SetGroupNotification(long groupUin, ulong sequence, BotGroupNotificationType type, bool isFiltered, GroupNotificationOperate operate, string message)
    {
        if (isFiltered)
        {
            await context.EventContext.SendEvent<SetFilteredGroupNotificationEventResp>(
                new SetFilteredGroupNotificationEventReq(
                    groupUin,
                    sequence,
                    type,
                    operate,
                    message
                )
            );
        }
        else
        {
            await context.EventContext.SendEvent<SetGroupNotificationEventResp>(
                new SetGroupNotificationEventReq(
                    groupUin,
                    sequence,
                    type,
                    operate,
                    message
                )
            );
        }
    }

    public async Task SetGroupReaction(long groupUin, ulong sequence, string code, bool isAdd)
    {
        if (isAdd) await context.EventContext.SendEvent<AddGroupReactionEventResp>(
            new AddGroupReactionEventReq(groupUin, sequence, code)
        );
        else await context.EventContext.SendEvent<ReduceGroupReactionEventResp>(
            new ReduceGroupReactionEventReq(groupUin, sequence, code)
        );
    }

    public async Task<string> GetNTV2RichMediaUrl(string uuid)
    {
        int remainder = uuid.Length % 4;
        int length = remainder == 0 ? uuid.Length : uuid.Length + (4 - remainder);
        string base64 = uuid.Replace('-', '+').Replace('_', '/').PadRight(length, '=');

        ulong appId = 0;
        uint ttl = 0;
        var reader = new ProtoReader(Convert.FromBase64String(base64));
        while (!reader.IsCompleted)
        {
            uint tag = reader.DecodeVarIntUnsafe<uint>();
            switch (tag >>> 3)
            {
                case 4:
                {
                    appId = reader.DecodeVarInt<ulong>();
                    break;
                }
                case 10:
                {
                    ttl = reader.DecodeVarInt<uint>();
                    break;
                }
                default:
                {
                    reader.SkipField((WireType)(tag & 0b111));
                    break;
                }
            }
        }

        NTV2RichMediaDownloadEventResp response = appId switch
        {
            // Record
            1402 => await context.EventContext.SendEvent<RecordDownloadEventResp>(new RecordDownloadEventReq(
                CreateFakeFriendMessage(context.BotUin),
                CreateFakeEntity<RecordEntity>(uuid, ttl)
            )),
            1403 => await context.EventContext.SendEvent<RecordGroupDownloadEventResp>(new RecordGroupDownloadEventReq(
                CreateFakeGroupMessage(context.BotUin),
                CreateFakeEntity<RecordEntity>(uuid, ttl)
            )),
            // Video
            1413 => await context.EventContext.SendEvent<VideoDownloadEventResp>(new VideoDownloadEventReq(
                CreateFakeFriendMessage(context.BotUin),
                CreateFakeEntity<VideoEntity>(uuid, ttl)
            )),
            1415 => await context.EventContext.SendEvent<VideoGroupDownloadEventResp>(new VideoGroupDownloadEventReq(
                CreateFakeGroupMessage(context.BotUin),
                CreateFakeEntity<VideoEntity>(uuid, ttl)
            )),
            // Image
            1406 => await context.EventContext.SendEvent<ImageDownloadEventResp>(new ImageDownloadEventReq(
                CreateFakeFriendMessage(context.BotUin),
                CreateFakeEntity<ImageEntity>(uuid, ttl)
            )),
            1407 => await context.EventContext.SendEvent<ImageGroupDownloadEventResp>(new ImageGroupDownloadEventReq(
                CreateFakeGroupMessage(context.BotUin),
                CreateFakeEntity<ImageEntity>(uuid, ttl)
            )),
            _ => throw new NotSupportedException($"Unsupported AppId: {appId}"),
        };

        return response.Url;

        static BotMessage CreateFakeFriendMessage(long uin) => BotMessage.CreateCustomFriend(uin, string.Empty, uin, string.Empty, DateTime.Now, []);
        static BotMessage CreateFakeGroupMessage(long uin) => BotMessage.CreateCustomGroup(uin, 0, string.Empty, DateTime.Now, []);
        static RichMediaEntityBase CreateFakeEntity<T>(string fileUuid, uint ttl) where T : RichMediaEntityBase, new()
        {
            return new T
            {
                MsgInfo = new MsgInfo
                {
                    MsgInfoBody = [new MsgInfoBody {
                        Index = new IndexNode {
                            Info = new FileInfo {},
                            FileUuid = fileUuid,
                            StoreId = 1,
                            UploadTime = 0,
                            Ttl = ttl,
                            SubType = 0,
                        }
                    }]
                }
            };
        }
    }
}
