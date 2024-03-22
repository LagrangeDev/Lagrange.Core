using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(VideoEntity))]
internal class VideoUploader : IHighwayUploader
{
    public async Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is VideoEntity { VideoStream: { } stream } video)
        {
            var uploadEvent = VideoUploadEvent.Create(video, chain.FriendInfo?.Uid ?? "");
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (VideoUploadEvent)uploadResult[0];

            if (metaResult.UKey != null)
            {
                var index = metaResult.MsgInfo.MsgInfoBody[0].Index;
                var extend = new NTV2RichMediaHighwayExt
                {
                    FileUuid = index.FileUuid,
                    UKey = metaResult.UKey,
                    Network = Common.Convert(metaResult.Network),
                    MsgInfoBody = metaResult.MsgInfo.MsgInfoBody,
                    BlockSize = 1024 * 1024,
                    Hash = new NTHighwayHash { FileSha1 = new List<byte[]> { index.Info.FileSha1.UnHex() } }
                };
                var extStream = extend.Serialize();

                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1001, stream, await Common.GetTicket(context), index.Info.FileHash.UnHex(), extStream.ToArray());
                if (!hwSuccess)
                {
                    await stream.DisposeAsync();
                    throw new Exception();
                }
            }

            video.MsgInfo = metaResult.MsgInfo; // directly constructed by Tencent's BDH Server
            video.Compat = metaResult.Compat; // for legacy QQ
            await stream.DisposeAsync();
        }
    }

    public async Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is VideoEntity { VideoStream: { } stream } video)
        {
            var uploadEvent = VideoGroupUploadEvent.Create(video, chain.GroupUin ?? 0);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (VideoGroupUploadEvent)uploadResult[0];
            
            if (metaResult.UKey != null)
            {
                var index = metaResult.MsgInfo.MsgInfoBody[0].Index;
                var extend = new NTV2RichMediaHighwayExt
                {
                    FileUuid = index.FileUuid,
                    UKey = metaResult.UKey,
                    Network = Common.Convert(metaResult.Network),
                    MsgInfoBody = metaResult.MsgInfo.MsgInfoBody,
                    BlockSize = 1024 * 1024,
                    Hash = new NTHighwayHash { FileSha1 = Common.CalculateStreamBytes(stream) }
                };
                var extStream = extend.Serialize();

                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1005, stream, await Common.GetTicket(context), index.Info.FileHash.UnHex(), extStream.ToArray());
                if (!hwSuccess)
                {
                    await stream.DisposeAsync();
                    throw new Exception();
                }
            }
            
            video.MsgInfo = metaResult.MsgInfo;  // directly constructed by Tencent's BDH Server
            video.Compat = metaResult.Compat;  // for legacy QQ
            await stream.DisposeAsync();
        }
    }
}