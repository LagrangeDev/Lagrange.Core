using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Context.Uploader;

internal class PttUploader : IHighwayUploader
{
    public Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        throw new NotImplementedException();
    }
}