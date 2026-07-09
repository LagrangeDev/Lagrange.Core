using Lagrange.Core.Events;

namespace Lagrange.Core.Services;

/// <summary>
/// Base class for a service with typed request and response events.
/// </summary>
public abstract class BaseService<TRequest, TResponse> : IService
    where TRequest : ProtocolEvent
    where TResponse : ProtocolEvent
{
    protected virtual ValueTask<TResponse> Parse(ReadOnlyMemory<byte> input, BotContext context) =>
        ValueTask.FromResult<TResponse>(null!);

    protected virtual ValueTask<ReadOnlyMemory<byte>> Build(TRequest input, BotContext context) =>
        ValueTask.FromResult(ReadOnlyMemory<byte>.Empty);

    async ValueTask<ProtocolEvent> IService.Parse(ReadOnlyMemory<byte> input, BotContext context) =>
        await Parse(input, context);

    ValueTask<ReadOnlyMemory<byte>> IService.Build(ProtocolEvent input, BotContext context) =>
        Build((TRequest)input, context);
}
