using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Context.Uploader;

internal interface IHighwayUploader
{
    public Task UploadPrivate(ContextCollection context, string uid, IMessageEntity entity);

    public Task UploadGroup(ContextCollection context, uint uin, IMessageEntity entity);
}