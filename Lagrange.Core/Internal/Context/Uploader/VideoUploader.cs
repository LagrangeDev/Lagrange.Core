using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(VideoEntity))]
internal class VideoUploader : IHighwayUploader
{
    public Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        throw new NotImplementedException();

    }

    public async Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is VideoEntity { VideoStream: { } stream } video)
        {
            var uploadEvent = VideoGroupUploadEvent.Create(video, chain.GroupUin ?? 0);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (VideoGroupUploadEvent)uploadResult[0];

            var hwUrlEvent = HighwayUrlEvent.Create();
            var highwayUrlResult = await context.Business.SendEvent(hwUrlEvent);
            var ticketResult = (HighwayUrlEvent)highwayUrlResult[0];
            
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
                    Hash = new NTHighwayHash { FileSha1 = index.Info.FileSha1.UnHex() }
                };
                var extStream = extend.Serialize();

                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1005, stream, ticketResult.SigSession, index.Info.FileHash.UnHex(), extStream.ToArray());
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