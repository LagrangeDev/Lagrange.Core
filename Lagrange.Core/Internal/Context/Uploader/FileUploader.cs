using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(FileEntity))]
internal class FileUploader : IHighwayUploader
{
    public Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        throw new NotImplementedException();
    }

    public async Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        if (entity is FileEntity { FileStream: not null } file)
        {
            var uploadEvent = GroupFSUploadEvent.Create(chain.GroupUin ?? 0, file);
            var result = await context.Business.SendEvent(uploadEvent);
        }
    }
}