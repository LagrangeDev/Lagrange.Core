using System.Collections.Frozen;
using System.Reflection;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Services;

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

        var services = new Dictionary<string, IService>(StringComparer.Ordinal);
        var servicesEventType = new Dictionary<Type, (ServiceAttribute, IService)>();
        AddBuiltInServices(context.Config.Protocol, services, servicesEventType);
        AddCustomServices(context.Config.Protocol, context.Config.CustomServices, services, servicesEventType);

        _services = services.ToFrozenDictionary(StringComparer.Ordinal);
        _servicesEventType = servicesEventType.ToFrozenDictionary();
    }

    public ValueTask<ProtocolEvent> Resolve(BotSsoPacket ssoPacket)
    {
        if (!_services.TryGetValue(ssoPacket.Command, out var service)) throw new ServiceNotFoundException(ssoPacket.Command);

        if (!_disabledLog.Contains(ssoPacket.Command)) _context.LogTrace(Tag, "Incoming SSOFrame: {0}", ssoPacket.Command);
        return service.Parse(ssoPacket.Data, _context);
    }

    public async ValueTask<(BotSsoPacket, ServiceAttribute)> Resolve(ProtocolEvent @event)
    {
        if (!_servicesEventType.TryGetValue(@event.GetType(), out var handler)) throw new ServiceNotFoundException(@event.GetType());

        var (attr, service) = handler;
        if (!handler.Attribute.DisableLog) _context.LogTrace(Tag, "Outgoing SSOFrame: {0}", handler.Attribute.Command);

        return (new BotSsoPacket(attr.Command, await service.Build(@event, _context), GetNewSequence()), attr);
    }

    public int GetNewSequence()
    {
        Interlocked.CompareExchange(ref _sequence, 5000000, 9900000);
        return Interlocked.Increment(ref _sequence);
    }

    private void AddBuiltInServices(
        Protocols protocol,
        Dictionary<string, IService> services,
        Dictionary<Type, (ServiceAttribute, IService)> servicesEventType)
    {
        var (builtInServices, builtInEventTypes) = ServiceRegistry.Create(protocol, _disabledLog);
        foreach ((string command, var service) in builtInServices) services.Add(command, service);

        foreach (var (eventType, registration) in builtInEventTypes)
        {
            servicesEventType.Add(eventType, registration);
        }
    }

    private void AddCustomServices(
        Protocols protocol,
        IReadOnlyList<IService> customServices,
        Dictionary<string, IService> services,
        Dictionary<Type, (ServiceAttribute, IService)> servicesEventType)
    {
        ArgumentNullException.ThrowIfNull(customServices);

        foreach (var service in customServices.ToArray())
        {
            if (service is null) throw new ServiceRegistrationException("Custom protocol services cannot contain null entries.");

            var serviceType = service.GetType();
            var attribute = serviceType.GetCustomAttribute<ServiceAttribute>(false) ?? throw new ServiceRegistrationException($"Custom protocol service {serviceType} must have {nameof(ServiceAttribute)}.");
            if (string.IsNullOrWhiteSpace(attribute.Command)) throw new ServiceRegistrationException($"Custom protocol service {serviceType} must specify a non-empty command.");

            var subscriptions = serviceType.GetCustomAttributes<EventSubscribeAttribute>(false).ToArray();
            ValidateSubscriptions(serviceType, subscriptions);
            
            var activeSubscriptions = subscriptions.Where(subscription => IsActive(subscription.Protocols, protocol)).ToArray();
            if (subscriptions.Length > 0 && activeSubscriptions.Length == 0) continue;

            if (!services.TryAdd(attribute.Command, service)) throw new ServiceRegistrationException($"Multiple protocol services are registered for command '{attribute.Command}'.");

            if (attribute.DisableLog) _disabledLog.Add(attribute.Command);

            foreach (var subscription in activeSubscriptions)
            {
                var eventType = subscription.EventType;
                if (servicesEventType.ContainsKey(eventType)) throw new ServiceRegistrationException($"Multiple protocol services are registered for event type '{eventType}'.");

                servicesEventType.Add(eventType, (attribute, service));
            }
        }
    }

    private static void ValidateSubscriptions(Type serviceType, EventSubscribeAttribute[] subscriptions)
    {
        var eventTypes = new HashSet<Type>();
        foreach (var subscription in subscriptions)
        {
            if (!typeof(ProtocolEvent).IsAssignableFrom(subscription.EventType)) throw new ServiceRegistrationException($"Event type '{subscription.EventType}' on {serviceType} must derive from {nameof(ProtocolEvent)}.");
            if (subscription.Protocols == Protocols.None) throw new ServiceRegistrationException($"Event subscription '{subscription.EventType}' on {serviceType} must specify at least one protocol.");
            if (!eventTypes.Add(subscription.EventType)) throw new ServiceRegistrationException($"Custom protocol service {serviceType} registers event type '{subscription.EventType}' more than once.");
        }
    }

    private static bool IsActive(Protocols supported, Protocols selected) => (~supported & selected) == Protocols.None;
}
