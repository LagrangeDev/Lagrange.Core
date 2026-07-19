using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Milky.Api.Handlers;

public interface IApiHandler
{
    Type RequestType { get; }

    ValueTask<MilkyApiResponse> HandleAsync(object request, CancellationToken ct);
}

public interface IApiHandler<TRequest, TResult> : IApiHandler where TRequest : notnull where TResult : notnull
{
    Type IApiHandler.RequestType => typeof(TRequest);

    ValueTask<MilkyApiResponse<TResult>> HandleAsync(TRequest request, CancellationToken ct);
    async ValueTask<MilkyApiResponse> IApiHandler.HandleAsync(object request, CancellationToken ct)
    {
        return await HandleAsync((TRequest)request, ct);
    }
}

public interface INoRequestApiHandler<TResult> : IApiHandler where TResult : notnull
{
    Type IApiHandler.RequestType => typeof(object);

    ValueTask<MilkyApiResponse<TResult>> HandleAsync(CancellationToken ct);
    async ValueTask<MilkyApiResponse> IApiHandler.HandleAsync(object request, CancellationToken ct)
    {
        return await HandleAsync(ct);
    }
}

public interface INoResultApiHandler<TRequest> : IApiHandler
{
    Type IApiHandler.RequestType => typeof(TRequest);

    ValueTask<MilkyApiResponse> HandleAsync(TRequest request, CancellationToken ct);
    async ValueTask<MilkyApiResponse> IApiHandler.HandleAsync(object request, CancellationToken ct)
    {
        return await HandleAsync((TRequest)request, ct);
    }
}