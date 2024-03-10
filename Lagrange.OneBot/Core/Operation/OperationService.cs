using System.Reflection;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Operation.Message;
using Lagrange.OneBot.Message;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lagrange.OneBot.Core.Operation;

public sealed class OperationService
{
    private readonly BotContext _bot;
    private readonly ILogger _logger;
    private readonly Dictionary<string, Type> _operations;
    private readonly ServiceProvider _service;

    public OperationService(BotContext bot, ILogger<OperationService> logger, LiteDatabase context, MessageService message)
    {
        _bot = bot;
        _logger = logger;

        _operations = new Dictionary<string, Type>();
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attributes = type.GetCustomAttributes<OperationAttribute>();
            foreach (var attribute in attributes) _operations[attribute.Api] = type;
        }

        var service = new ServiceCollection();
        service.AddSingleton(context);
        service.AddSingleton(logger);
        service.AddSingleton(message);
        service.AddSingleton<MessageCommon>();
        service.AddLogging();

        foreach (var (_, type) in _operations) service.AddScoped(type);
        _service = service.BuildServiceProvider();
    }

    public async Task<OneBotResult?> HandleOperation(MsgRecvEventArgs e)
    {
        try
        {
            if (JsonSerializer.Deserialize<OneBotAction>(e.Data) is { } action)
            {
                try
                {
                    if (_operations.TryGetValue(action.Action, out var type))
                    {
                        var handler = (IOperation)_service.GetRequiredService(type);
                        var result = await handler.HandleOperation(_bot, action.Params);
                        result.Echo = action.Echo;

                        return result;
                    }

                    return new OneBotResult(null, 404, "failed") { Echo = action.Echo };
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Unexpected error encountered while handling message.");
                    return new OneBotResult(null, 200, "failed") { Echo = action.Echo };
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Json Serialization failed for such action");
        }

        return null;
    }
}