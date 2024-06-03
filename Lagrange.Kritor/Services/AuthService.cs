using Grpc.Core;
using Kritor.Authentication;

namespace Lagrange.Kritor.Services;

public class AuthService : global::Kritor.Authentication.AuthenticationService.AuthenticationServiceBase
{
    public override Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context)
    {
        return base.Authenticate(request, context);
    }

    public override Task<AddTicketResponse> AddTicket(AddTicketRequest request, ServerCallContext context)
    {
        return base.AddTicket(request, context);
    }

    public override Task<DeleteTicketResponse> DeleteTicket(DeleteTicketRequest request, ServerCallContext context)
    {
        return base.DeleteTicket(request, context);
    }

    public override Task<GetTicketResponse> GetTicket(GetTicketRequest request, ServerCallContext context)
    {
        return base.GetTicket(request, context);
    }

    public override Task<GetAuthenticationStateResponse> GetAuthenticationState(GetAuthenticationStateRequest request, ServerCallContext context)
    {
        return base.GetAuthenticationState(request, context);
    }
}