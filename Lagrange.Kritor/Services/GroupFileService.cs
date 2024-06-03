using Grpc.Core;
using Kritor.File;

namespace Lagrange.Kritor.Services;

public class GroupFileService : global::Kritor.File.GroupFileService.GroupFileServiceBase
{
    public override Task<CreateFolderResponse> CreateFolder(CreateFolderRequest request, ServerCallContext context)
    {
        return base.CreateFolder(request, context);
    }

    public override Task<RenameFolderResponse> RenameFolder(RenameFolderRequest request, ServerCallContext context)
    {
        return base.RenameFolder(request, context);
    }

    public override Task<DeleteFolderResponse> DeleteFolder(DeleteFolderRequest request, ServerCallContext context)
    {
        return base.DeleteFolder(request, context);
    }

    public override Task<UploadFileResponse> UploadFile(UploadFileRequest request, ServerCallContext context)
    {
        return base.UploadFile(request, context);
    }

    public override Task<DeleteFileResponse> DeleteFile(DeleteFileRequest request, ServerCallContext context)
    {
        return base.DeleteFile(request, context);
    }

    public override Task<GetFileSystemInfoResponse> GetFileSystemInfo(GetFileSystemInfoRequest request, ServerCallContext context)
    {
        return base.GetFileSystemInfo(request, context);
    }

    public override Task<GetFileListResponse> GetFileList(GetFileListRequest request, ServerCallContext context)
    {
        return base.GetFileList(request, context);
    }
}