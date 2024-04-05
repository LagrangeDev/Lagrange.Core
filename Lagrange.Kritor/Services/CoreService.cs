using Grpc.Core;
using Kritor.Core;

namespace Lagrange.Kritor.Services;

public class CoreService : global::Kritor.Core.CoreService.CoreServiceBase
{
    public override Task<GetVersionResponse> GetVersion(GetVersionRequest request, ServerCallContext context)
    {
        return base.GetVersion(request, context);
    }

    public override Task<DownloadFileResponse> DownloadFile(DownloadFileRequest request, ServerCallContext context)
    {
        return base.DownloadFile(request, context);
    }

    public override Task<GetCurrentAccountResponse> GetCurrentAccount(GetCurrentAccountRequest request, ServerCallContext context)
    {
        return base.GetCurrentAccount(request, context);
    }

    public override Task<SwitchAccountResponse> SwitchAccount(SwitchAccountRequest request, ServerCallContext context)
    {
        return base.SwitchAccount(request, context);
    }
}