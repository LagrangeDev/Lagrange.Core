using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Context.Uploader;

internal interface IHighwayUploader
{
    public Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity);

    public Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity);
}