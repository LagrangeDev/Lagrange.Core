using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Context.Uploader;

[HighwayUploader(typeof(RecordEntity))]
internal class PttUploader : IHighwayUploader
{
    private const string Tag = nameof(PttUploader);
    
    public Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity)
    {
        throw new NotImplementedException();
    }
}