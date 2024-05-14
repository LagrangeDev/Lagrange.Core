using Grpc.Core;
using Kritor.Common;

namespace Lagrange.Kritor.Services;

public class ReverseService : global::Kritor.Reverse.ReverseService.ReverseServiceBase
{
    public override Task ReverseStream(IAsyncStreamReader<Response> requestStream, IServerStreamWriter<Request> responseStream, ServerCallContext context)
    {
        return base.ReverseStream(requestStream, responseStream, context);
    }
}