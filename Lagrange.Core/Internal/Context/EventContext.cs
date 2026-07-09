using System.Collections.Frozen;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Logic;

namespace Lagrange.Core.Internal.Context;

internal class EventContext : IDisposable
{
    private const string Tag = nameof(EventContext);

    private readonly BotContext _context;

    private readonly FrozenDictionary<Type, List<ILogic>> _events;

    private readonly FrozenDictionary<Type, ILogic> _logics;

    public EventContext(BotContext context)
    {
        _context = context;
        (_events, _logics) = EventLogicRegistry.Create(context);
    }

    public async ValueTask<T> SendEvent<T>(ProtocolEvent @event) where T : ProtocolEvent
    {
        try
        {
            await HandleOutgoingEvent(@event);
            var (frame, attribute) = await _context.ServiceContext.Resolve(@event);
            if (frame.Sequence == 0) throw new LagrangeException("The sequence number is 0 for the SSOFrame");


            var @return = await _context.PacketContext.SendPacket(frame, attribute);
            var resolved = await _context.ServiceContext.Resolve(@return);

            if (resolved is T result)
            {
                await HandleIncomingEvent(result);
                return result;
            }

            throw new LagrangeException($"The event type is not the same as the expected type. Expected: {typeof(T)}, Actual: {resolved.GetType()}");
        }
        catch (Exception e) when (e is not LagrangeException)
        {
            throw new LagrangeException("An error occurred while sending the event", e);
        }
    }

    private async ValueTask HandleIncomingEvent(ProtocolEvent @event)
    {
        if (_events.TryGetValue(@event.GetType(), out var logics))
        {
            foreach (var logic in logics)
            {
                try
                {
                    await logic.Incoming(@event);
                }
                catch (Exception e) when (e is not LagrangeException)
                {
                    throw new LagrangeException("An error occurred while processing the incoming event", e);
                }
            }
        }
    }

    private async ValueTask HandleOutgoingEvent(ProtocolEvent @event)
    {
        if (_events.TryGetValue(@event.GetType(), out var logics))
        {
            foreach (var logic in logics)
            {
                try
                {
                    await logic.Outgoing(@event);
                }
                catch (Exception e) when (e is not LagrangeException)
                {
                    throw new LagrangeException("An error occurred while processing the outgoing event", e);
                }
            }
        }
    }

    public async ValueTask HandleOutgoingEvent(EventBase @event)
    {
        if (_events.TryGetValue(@event.GetType(), out var logics))
        {
            foreach (var logic in logics)
            {
                try
                {
                    await logic.Outgoing(@event);
                }
                catch (Exception e) when (e is not LagrangeException)
                {
                    throw new LagrangeException("An error occurred while processing the outgoing event", e);
                }
            }
        }
    }

    public async Task HandleServerPacket(BotSsoPacket packet)
    {
        try
        {
            var @event = await _context.ServiceContext.Resolve(packet);
            await HandleIncomingEvent(@event);
        }
        catch (ServiceNotFoundException e)
        {
            _context.LogDebug(Tag, "Service not found for command: {0}", e, e.Command);
        }
        catch (Exception e)
        {
            _context.LogError(Tag, "Handle {0} server packet failed", e, packet.Command);
        }
    }

    public T GetLogic<T>() where T : ILogic => (T)_logics[typeof(T)];

    public void Dispose()
    {
        foreach (var logic in _logics.Values)
        {
            if (logic is IDisposable disposable) disposable.Dispose();
        }
    }
}
