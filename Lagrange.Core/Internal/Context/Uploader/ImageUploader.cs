using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(ImageEntity))]
internal class ImageUploader : IHighwayUploader
{
    private const string Tag = nameof(ImageUploader);
    
    public async Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is ImageEntity { ImageStream: not null } image)
        {
            string uid = await context.Business.CachingLogic.ResolveUid(chain.GroupUin, chain.FriendUin) ?? 
                         throw new Exception($"Failed to resolve Uid for Uin {chain.FriendUin}");
            var @event = ImageUploadEvent.Create(image.ImageStream, uid);
            var results = await context.Business.SendEvent(@event);
            if (results.Count != 0)
            {
                var ticketResult = (ImageUploadEvent)results[0];
                if (!ticketResult.IsExist)
                {
                    bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(1, image.ImageStream, ticketResult.Ticket, @event.FileMd5.UnHex());
                    if (!hwSuccess) throw new Exception();
                }

                image.ImageStream = @event.Stream;
                image.Path = ticketResult.ServerPath;
            }
        }
    }

    public async Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is ImageEntity { ImageStream: not null } image)
        {
            var @event = ImageGroupUploadEvent.Create(image.ImageStream, chain.GroupUin ?? throw new Exception());
            var results = await context.Business.SendEvent(@event);
            if (results.Count != 0)
            {
                var ticketResult = (ImageGroupUploadEvent)results[0];
                if (!ticketResult.IsExist)
                {
                    bool hwSuccess = await context.Highway.UploadSrcByStreamAsync(2, image.ImageStream, ticketResult.Ticket, @event.FileMd5.UnHex());
                    if (!hwSuccess) throw new Exception();
                }

                image.ImageStream = @event.Stream;
                image.FileId = ticketResult.FileId;
            }
        }
    }
}