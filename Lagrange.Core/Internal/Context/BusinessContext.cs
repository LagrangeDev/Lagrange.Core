using System.Reflection;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Context.Logic;
using Lagrange.Core.Internal.Context.Logic.Implementation;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Service;
using Lagrange.Core.Utility.Extension;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Context;

internal class BusinessContext : ContextBase
{
    private const string Tag = nameof(BusinessContext);
    
    private readonly Dictionary<Type, List<LogicBase>> _businessLogics;
    
    #region Business Logics
    
    internal MessagingLogic MessagingLogic { get; private set; }
    
    internal WtExchangeLogic WtExchangeLogic { get; private set; }
    
    internal OperationLogic OperationLogic { get; private set; }
    
    internal CachingLogic CachingLogic { get; private set; }

    #endregion

    public BusinessContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device) 
        : base(collection, keystore, appInfo, device)
    {
        _businessLogics = new Dictionary<Type, List<LogicBase>>();
        
        RegisterLogics();
    }

    private void RegisterLogics()
    {
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var logic in assembly.GetTypeByAttributes<BusinessLogicAttribute>(out _))
        {
            var constructor = logic.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            var instance = (LogicBase)constructor[0].Invoke(new object[] { Collection });
            
            foreach (var @event in logic.GetCustomAttributes<EventSubscribeAttribute>())
            {
                if (!_businessLogics.TryGetValue(@event.EventType, out var list))
                {
                    list = new List<LogicBase>();
                    _businessLogics.Add(@event.EventType, list);
                }
                list.Add(instance); // Append logics
            }

            switch (instance)
            {
                case WtExchangeLogic wtExchangeLogic:
                    WtExchangeLogic = wtExchangeLogic;
                    break;
                case MessagingLogic messagingLogic:
                    MessagingLogic = messagingLogic;
                    break;
                case OperationLogic operationLogic:
                    OperationLogic = operationLogic;
                    break;
                case CachingLogic cachingLogic:
                    CachingLogic = cachingLogic;
                    break;
            }
        }
    }

    public async Task<bool> PushEvent(ProtocolEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var packets = Collection.Service.ResolvePacketByEvent(@event);
            foreach (var packet in packets)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Collection.Packet.PostPacket(packet);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Send Event to the Server, goes through the given context
    /// </summary>
    public async Task<List<ProtocolEvent>> SendEvent(ProtocolEvent @event, CancellationToken cancellationToken)
    {
        await HandleOutgoingEvent(@event, cancellationToken);
        var result = new List<ProtocolEvent>();

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var packets = Collection.Service.ResolvePacketByEvent(@event);
            foreach (var packet in packets)
            {
                var returnVal = await Collection.Packet.SendPacket(packet, cancellationToken);
                var resolved = Collection.Service.ResolveEventByPacket(returnVal);
                foreach (var protocol in resolved)
                {
                    await HandleIncomingEvent(protocol, cancellationToken);
                    result.Add(protocol);
                }
            }
        }
        catch (TaskCanceledException)
        {
            // if task is cancelled, we should throw the exception
            throw;
        }
        catch (Exception e)
        {
            Collection.Log.LogWarning(Tag, $"Error when processing the event: {@event}");
            Collection.Log.LogWarning(Tag, e.ToString());
        }
        
        return result;
    }

    public async Task<bool> HandleIncomingEvent(ProtocolEvent @event, CancellationToken cancellationToken)
    {
        _businessLogics.TryGetValue(typeof(ProtocolEvent), out var baseLogics);
        _businessLogics.TryGetValue(@event.GetType(), out var normalLogics);

        var logics = new List<LogicBase>();
        if (baseLogics != null) logics.AddRange(baseLogics);
        if (normalLogics != null) logics.AddRange(normalLogics);

        foreach (var logic in logics)
        {
            try
            {
                await logic.Incoming(@event, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // if task is cancelled, we should throw the exception
                throw;
            }
            catch (Exception e)
            {
                Collection.Log.LogFatal(Tag, $"Error occurred while handling event {@event.GetType().Name}");
                Collection.Log.LogFatal(Tag, e.Message);
                if (e.StackTrace != null) Collection.Log.LogFatal(Tag, e.StackTrace);
            }
        }
        
        return true;
    }
    
    public async Task<bool> HandleOutgoingEvent(ProtocolEvent @event, CancellationToken cancellationToken)
    {
        _businessLogics.TryGetValue(typeof(ProtocolEvent), out var baseLogics);
        _businessLogics.TryGetValue(@event.GetType(), out var normalLogics);

        var logics = new List<LogicBase>();
        if (baseLogics != null) logics.AddRange(baseLogics);
        if (normalLogics != null) logics.AddRange(normalLogics);

        foreach (var logic in logics)
        {
            try
            {
                await logic.Outgoing(@event, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // if task is cancelled, we should throw the exception
                throw;
            }
            catch (Exception e)
            {
                Collection.Log.LogFatal(Tag, $"Error occurred while handling outgoing event {@event.GetType().Name}");
                Collection.Log.LogFatal(Tag, e.Message);
                if (e.StackTrace != null) Collection.Log.LogFatal(Tag, e.StackTrace);
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Handle the incoming packet with new sequence number.
    /// </summary>
    public async Task<bool> HandleServerPacket(SsoPacket packet)
    {
        bool success = false;

        try
        {
            var events = Collection.Service.ResolveEventByPacket(packet);
            foreach (var @event in events)
            {
                var isSuccessful = await Collection.Business.HandleIncomingEvent(@event, CancellationToken.None);
                if (!isSuccessful) break;

                success = true;
            }
        }
        catch (Exception e)
        {
            Collection.Log.LogWarning(Tag, $"Error while handling msf push: {packet.PacketType} {packet.Command}");
            Collection.Log.LogWarning(Tag, e.Message);
            if (e.StackTrace is { } stackTrace) Collection.Log.LogWarning(Tag, stackTrace);
            Collection.Log.LogDebug(Tag, packet.Payload.ToArray().Hex());
        }

        return success;
    }
}
