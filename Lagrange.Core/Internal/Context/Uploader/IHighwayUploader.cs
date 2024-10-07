using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Context.Uploader;

internal interface IHighwayUploader
{
    public Task UploadPrivate(ContextCollection context, MessageChain chain, IMessageEntity entity, CancellationToken ct);
    
    public Task UploadGroup(ContextCollection context, MessageChain chain, IMessageEntity entity, CancellationToken ct);
}