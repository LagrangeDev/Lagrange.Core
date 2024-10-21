using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(VideoEntity))]
internal class VideoUploader : IHighwayUploader
{
    public async Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is VideoEntity { VideoStream: { } stream, ThumbnailStream: { } thumbnail } video)
        {
            var uploadEvent = VideoUploadEvent.Create(video, chain.FriendInfo?.Uid ?? "");
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (VideoUploadEvent)uploadResult[0];

            if (Common.GenerateExt(metaResult) is { } ext)
            {
                ext.Hash.FileSha1 = Common.CalculateStreamBytes(stream.Value);

                var hash = metaResult.MsgInfo.MsgInfoBody[0].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1001, stream.Value, await Common.GetTicket(context), hash, ext.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    throw new Exception();
                }
            }
            
            if (Common.GenerateExt(metaResult, metaResult.SubFiles[0]) is { } subExt)
            {
                var hash = metaResult.MsgInfo.MsgInfoBody[1].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1002, thumbnail.Value, await Common.GetTicket(context), hash, subExt.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    await thumbnail.Value.DisposeAsync();
                    throw new Exception();
                }
            }

            video.MsgInfo = metaResult.MsgInfo; // directly constructed by Tencent's BDH Server
            video.Compat = metaResult.Compat; // for legacy QQ
            await stream.Value.DisposeAsync();
            await thumbnail.Value.DisposeAsync();
        }
    }

    public async Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is VideoEntity { VideoStream: { } stream, ThumbnailStream: { } thumbnail } video)
        {
            var uploadEvent = VideoGroupUploadEvent.Create(video, chain.GroupUin ?? 0);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (VideoGroupUploadEvent)uploadResult[0];
            
            if (Common.GenerateExt(metaResult) is { } ext)
            {
                ext.Hash.FileSha1 = Common.CalculateStreamBytes(stream.Value);
                
                var hash = metaResult.MsgInfo.MsgInfoBody[0].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1005, stream.Value, await Common.GetTicket(context), hash, ext.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    throw new Exception();
                }
            }

            if (Common.GenerateExt(metaResult, metaResult.SubFiles[0]) is { } subExt)
            {
                var hash = metaResult.MsgInfo.MsgInfoBody[1].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1006, thumbnail.Value, await Common.GetTicket(context), hash, subExt.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    await thumbnail.Value.DisposeAsync();
                    throw new Exception();
                }
            }
            
            video.MsgInfo = metaResult.MsgInfo;  // directly constructed by Tencent's BDH Server
            video.Compat = metaResult.Compat;  // for legacy QQ
            await stream.Value.DisposeAsync();
            await thumbnail.Value.DisposeAsync();
        }
    }
}