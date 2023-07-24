using System.Reflection;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Service;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Lagrange.Core.Core.Context;

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
                uint sequence = (uint)_sequenceProvider.GetNewSequence();
                instance.SourceEvents[sequence] = protocolEvent;
                result.Add(new SsoPacket(attribute.PacketType, attribute.Command, sequence, binary));
                
                if (extraPackets != null)
                {
                    result.AddRange(extraPackets.Select(extra =>
                    {
                        uint extraSequence = (uint)_sequenceProvider.GetNewSequence();
                        instance.SourceEvents[extraSequence] = protocolEvent;
                        return new SsoPacket(attribute.PacketType, attribute.Command, extraSequence, extra);
                    }));
                }
                
                Collection.Log.LogVerbose(Tag, $"Outgoing SSOFrame: {attribute.Command}");
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
        if (!_services.TryGetValue(packet.Command, out var service))
        {
            Collection.Log.LogWarning(Tag, $"Unsupported SSOFrame Received: {packet.Command}");
            var payload = packet.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
            Collection.Log.LogDebug(Tag, $"Unsuccessful SSOFrame Payload: {payload.Hex()}");
            return result; // 没找到 滚蛋吧
        }

        bool success = service.Parse(packet, Keystore, AppInfo, DeviceInfo, out var @event, out var extraEvents);
        service.SourceEvents.Remove(packet.Sequence);

        if (success)
        {
            result.Add(@event);
            if (extraEvents != null) result.AddRange(extraEvents);
            
            Collection.Log.LogVerbose(Tag, $"Incoming SSOFrame: {packet.Command}");
        }
        
        return result;
    }
}