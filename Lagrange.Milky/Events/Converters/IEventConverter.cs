using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events;

namespace Lagrange.Milky.Events.Converters;

public interface IEventConverter<TEvent> where TEvent : EventBase
{
    string Name { get; }

    bool CanConvert(TEvent @event);

    ValueTask<object> ConvertAsync(TEvent @event, CancellationToken ct);
}

public interface IEventConverter<TEvent, TData> : IEventConverter<TEvent> where TEvent : EventBase where TData : class
{
    ValueTask<object> IEventConverter<TEvent>.ConvertAsync(TEvent @event, CancellationToken ct)
    {
        var vt = ConvertAsync(@event, ct);
        return vt.IsCompleted ? ValueTask.FromResult<object>(vt.Result) : CastAwait(vt);

        static async ValueTask<object> CastAwait(ValueTask<TData> vt) => await vt;
    }
    new ValueTask<TData> ConvertAsync(TEvent @event, CancellationToken ct);
}