using System.Collections.Frozen;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Services;

namespace Lagrange.Core.Internal.Context;

internal class ServiceContext
{
    private const string Tag = nameof(ServiceContext);

    private int _sequence = Random.Shared.Next(5000000, 9900000);

    private readonly HashSet<string> _disabledLog = [];
    private readonly FrozenDictionary<string, IService> _services;
    private readonly FrozenDictionary<Type, (ServiceAttribute Attribute, IService Instance)> _servicesEventType;

    private readonly BotContext _context;

    public ServiceContext(BotContext context)
    {
        _context = context;

        (_services, _servicesEventType) = ServiceRegistry.Create(context.Config.Protocol, _disabledLog);
    }

    public ValueTask<ProtocolEvent> Resolve(BotSsoPacket ssoPacket)
    {
        if (!_services.TryGetValue(ssoPacket.Command, out var service)) throw new ServiceNotFoundException(ssoPacket.Command);

        if (!_disabledLog.Contains(ssoPacket.Command)) _context.LogTrace(Tag, "Incoming SSOFrame: {0}", ssoPacket.Command);
        return service.Parse(ssoPacket.Data, _context);
    }

    public async ValueTask<(BotSsoPacket, ServiceAttribute)> Resolve(ProtocolEvent @event)
    {
        if (!_servicesEventType.TryGetValue(@event.GetType(), out var handler)) return default;

        var (attr, service) = handler;
        if (!handler.Attribute.DisableLog) _context.LogTrace(Tag, "Outgoing SSOFrame: {0}", handler.Attribute.Command);

        return (new BotSsoPacket(attr.Command, await service.Build(@event, _context), GetNewSequence()), attr);
    }

    public int GetNewSequence()
    {
        Interlocked.CompareExchange(ref _sequence, 5000000, 9900000);
        return Interlocked.Increment(ref _sequence);
    }
}
