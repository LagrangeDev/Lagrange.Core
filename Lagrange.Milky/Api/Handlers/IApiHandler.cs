using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Milky.Api.Handlers;

public interface IApiHandler
{
    Type RequestType { get; }

    ValueTask<MilkyApiResponse> HandleAsync(object request, CancellationToken ct);
}

public interface IApiHandler<TRequest, TData> : IApiHandler where TRequest : notnull where TData : notnull
{
    Type IApiHandler.RequestType => typeof(TRequest);

    ValueTask<MilkyApiResponse<TData>> HandleAsync(TRequest request, CancellationToken ct);
    async ValueTask<MilkyApiResponse> IApiHandler.HandleAsync(object request, CancellationToken ct)
    {
        return await HandleAsync((TRequest)request, ct);
    }
}

public interface INoRequestApiHandler<TData> : IApiHandler where TData : notnull
{
    Type IApiHandler.RequestType => typeof(object);

    ValueTask<MilkyApiResponse<TData>> HandleAsync(CancellationToken ct);
    async ValueTask<MilkyApiResponse> IApiHandler.HandleAsync(object request, CancellationToken ct)
    {
        return await HandleAsync(ct);
    }
}

public interface INoResponseApiHandler<TRequest> : IApiHandler
{
    Type IApiHandler.RequestType => typeof(TRequest);

    ValueTask HandleAsync(TRequest request, CancellationToken ct);
    async ValueTask<MilkyApiResponse> IApiHandler.HandleAsync(object request, CancellationToken ct)
    {
        await HandleAsync((TRequest)request, ct);
        return MilkyApiResponse<object>.Ok(new object());
    }
}