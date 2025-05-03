using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(RecordEntity))]
internal class PttUploader : IHighwayUploader
{
    public async Task UploadPrivate(ContextCollection context, string uid, IMessageEntity entity)
    {
        if (entity is RecordEntity { AudioStream: { } stream } record)
        {
            var uploadEvent = RecordUploadEvent.Create(record, uid);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (RecordUploadEvent)uploadResult[0];

            if (Common.GenerateExt(metaResult) is { } ext)
            {
                var hash = metaResult.MsgInfo.MsgInfoBody[0].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1007, stream.Value, await Common.GetTicket(context), hash, ext.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    throw new Exception();
                }
            }

            record.MsgInfo = metaResult.MsgInfo;  // directly constructed by Tencent's BDH Server
            record.Compat = metaResult.Compat;  // for legacy QQ
            await record.AudioStream.Value.DisposeAsync();
        }
    }

    public async Task UploadGroup(ContextCollection context, uint uin, IMessageEntity entity)
    {
        if (entity is RecordEntity { AudioStream: { } stream } record)
        {
            var uploadEvent = RecordGroupUploadEvent.Create(record, uin);
            var uploadResult = await context.Business.SendEvent(uploadEvent);
            var metaResult = (RecordGroupUploadEvent)uploadResult[0];

            if (Common.GenerateExt(metaResult) is { } ext)
            {
                var hash = metaResult.MsgInfo.MsgInfoBody[0].Index.Info.FileHash.UnHex();
                bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1008, stream.Value, await Common.GetTicket(context), hash, ext.Serialize().ToArray());
                if (!hwSuccess)
                {
                    await stream.Value.DisposeAsync();
                    throw new Exception();
                }
            }

            record.MsgInfo = metaResult.MsgInfo;  // directly constructed by Tencent's BDH Server
            record.Compat = metaResult.Compat;  // for legacy QQ
            await record.AudioStream.Value.DisposeAsync();
        }
    }
}