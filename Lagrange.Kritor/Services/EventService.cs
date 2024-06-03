using Grpc.Core;
using Kritor.Event;

namespace Lagrange.Kritor.Services;

public class EventService : global::Kritor.Event.EventService.EventServiceBase
{
    public override Task RegisterActiveListener(RequestPushEvent request, IServerStreamWriter<EventStructure> responseStream, ServerCallContext context)
    {
        return base.RegisterActiveListener(request, responseStream, context);
    }

    public override Task<RequestPushEvent> RegisterPassiveListener(IAsyncStreamReader<EventStructure> requestStream, ServerCallContext context)
    {
        return base.RegisterPassiveListener(requestStream, context);
    }
}