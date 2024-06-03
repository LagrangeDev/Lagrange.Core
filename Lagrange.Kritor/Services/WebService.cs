using Grpc.Core;
using Kritor.Web;

namespace Lagrange.Kritor.Services;

public class WebService : global::Kritor.Web.WebService.WebServiceBase
{
    public override Task<GetCookiesResponse> GetCookies(GetCookiesRequest request, ServerCallContext context)
    {
        return base.GetCookies(request, context);
    }

    public override Task<GetCredentialsResponse> GetCredentials(GetCredentialsRequest request, ServerCallContext context)
    {
        return base.GetCredentials(request, context);
    }

    public override Task<GetCSRFTokenResponse> GetCSRFToken(GetCSRFTokenRequest request, ServerCallContext context)
    {
        return base.GetCSRFToken(request, context);
    }

    public override Task<GetHttpCookiesResponse> GetHttpCookies(GetHttpCookiesRequest request, ServerCallContext context)
    {
        return base.GetHttpCookies(request, context);
    }
}