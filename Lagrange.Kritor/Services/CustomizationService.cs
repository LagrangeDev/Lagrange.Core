using Grpc.Core;
using Kritor.Common;

namespace Lagrange.Kritor.Services;

public class CustomizationService : global::Kritor.Customization.CustomizationService.CustomizationServiceBase
{
    public override Task<Response> CallFunction(Request request, ServerCallContext context)
    {
        return base.CallFunction(request, context);
    }
}