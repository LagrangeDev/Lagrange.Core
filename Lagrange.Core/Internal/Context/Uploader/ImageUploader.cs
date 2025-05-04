using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(ImageEntity))]
internal class ImageUploader : IHighwayUploader
{
    public async Task UploadPrivate(ContextCollection context, string uid, IMessageEntity entity)
    {
        if (entity is ImageEntity { ImageStream: { } stream } image)
        {
            var uploadEvent = ImageUploadEvent.Create(image, uid);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (ImageUploadEvent)uploadResult[0];

            if (Common.GenerateExt(metaResult) is { } ext)
            {
                var hash = metaResult.MsgInfo.MsgInfoBody[0].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1003, stream.Value, await Common.GetTicket(context), hash, ext.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    throw new Exception();
                }
            }

            image.MsgInfo = metaResult.MsgInfo;  // directly constructed by Tencent's BDH Server
            image.CompatImage = metaResult.Compat;  // for legacy QQ
            await image.ImageStream.Value.DisposeAsync();
        }
    }

    public async Task UploadGroup(ContextCollection context, uint uin, IMessageEntity entity)
    {
        if (entity is ImageEntity { ImageStream: { } stream } image)
        {
            var uploadEvent = ImageGroupUploadEvent.Create(image, uin);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (ImageGroupUploadEvent)uploadResult[0];

            if (Common.GenerateExt(metaResult) is { } ext)
            {
                var hash = metaResult.MsgInfo.MsgInfoBody[0].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1004, stream.Value, await Common.GetTicket(context), hash, ext.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    throw new Exception();
                }
            }

            image.MsgInfo = metaResult.MsgInfo;  // directly constructed by Tencent's BDH Server
            image.CompatFace = metaResult.Compat;  // for legacy QQ
            await image.ImageStream.Value.DisposeAsync();
        }
    }
}