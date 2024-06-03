using System.Collections.Concurrent;
using System.Reflection;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Service;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context;

/// <summary>
/// <para>Manage the service and packet translation of the Bot</para>
/// <para>Instantiate the Service by <see cref="System.Reflection"/> and store such</para>
/// <para>Translate the event into <see cref="ProtocolEvent"/>, you may manually dispatch the packet to <see cref="PacketContext"/></para>
/// </summary>
internal partial class ServiceContext : ContextBase
{
    private const string Tag = nameof(ServiceContext);
    
    private readonly SequenceProvider _sequenceProvider;
    private readonly Dictionary<string, IService> _services;
    private readonly Dictionary<Type, List<(ServiceAttribute Attribute, IService Instance)>> _servicesEventType;

    public ServiceContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(collection, keystore, appInfo, device)
    {
        _sequenceProvider = new SequenceProvider();
        _services = new Dictionary<string, IService>();
        _servicesEventType = new Dictionary<Type, List<(ServiceAttribute, IService)>>();
        
        RegisterServices();
    }

    private void RegisterServices()
    {
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var type in assembly.GetDerivedTypes<ProtocolEvent>())
        {
            _servicesEventType[type] = new List<(ServiceAttribute, IService)>();
        }

        foreach (var type in assembly.GetTypeByAttributes<ServiceAttribute>(out _))
        {
            var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();

            if (serviceAttribute != null)
            {
                var service = (IService)type.CreateInstance();
                _services[serviceAttribute.Command] = service;
                
                foreach (var attribute in type.GetCustomAttributes<EventSubscribeAttribute>())
                {
                    _servicesEventType[attribute.EventType].Add((serviceAttribute, service));
                }
            }
        }
    }

    /// <summary>
    /// Resolve the outgoing packet by the event
    /// </summary>
    public List<SsoPacket> ResolvePacketByEvent(ProtocolEvent protocolEvent)
    {
        var result = new List<SsoPacket>();
        if (!_servicesEventType.TryGetValue(protocolEvent.GetType(), out var serviceList)) return result; // 没找到 滚蛋吧

        foreach (var (attribute, instance) in serviceList)
        {
            bool success = instance.Build(protocolEvent, Keystore, AppInfo, DeviceInfo, out var binary, out var extraPackets);

            if (success && binary != null)
            {
                result.Add(new SsoPacket(attribute.PacketType, attribute.Command, (uint)_sequenceProvider.GetNewSequence(), binary));
                
                if (extraPackets != null)
                {
                    result.AddRange(extraPackets.Select(extra => new SsoPacket(attribute.PacketType, attribute.Command, (uint)_sequenceProvider.GetNewSequence(), extra)));
                }
                
                Collection.Log.LogDebug(Tag, $"Outgoing SSOFrame: {attribute.Command}");
            }
        }

        return result;
    }
    
    /// <summary>
    /// Resolve the incoming event by the packet
    /// </summary>
    public List<ProtocolEvent> ResolveEventByPacket(SsoPacket packet)
    {
        var result = new List<ProtocolEvent>();
        var payload = packet.Payload.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix);
        
        if (!_services.TryGetValue(packet.Command, out var service))
        {
            Collection.Log.LogWarning(Tag, $"Unsupported SSOFrame Received: {packet.Command}");
            Collection.Log.LogDebug(Tag, $"Unsuccessful SSOFrame Payload: {payload.Hex()}");
            return result; // 没找到 滚蛋吧
        }

        bool success = service.Parse(payload, Keystore, AppInfo, DeviceInfo, out var @event, out var extraEvents);

        if (success)
        {
            if (@event != null) result.Add(@event);
            if (extraEvents != null) result.AddRange(extraEvents);
            
            Collection.Log.LogDebug(Tag, $"Incoming SSOFrame: {packet.Command}");
        }
        packet.Dispose();
        
        return result;
    }
    
    public int GetNewSequence() => _sequenceProvider.GetNewSequence();
    
    private class SequenceProvider
    {
        private readonly ConcurrentDictionary<string, int> _sessionSequence = new();
        
        private int _sequence = Random.Shared.Next(5000000, 9900000);

        public int GetNewSequence()
        {
            Interlocked.CompareExchange(ref _sequence, 5000000, 9900000);
            return Interlocked.Increment(ref _sequence);
        }
        
        public int RegisterSession(string sessionId) => _sessionSequence.GetOrAdd(sessionId, GetNewSequence());
    }
}