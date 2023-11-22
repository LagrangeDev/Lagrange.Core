using System.Reflection;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Network.Service;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Operation;

public sealed class OperationService
{
    private readonly BotContext _bot;
    private readonly ILogger _logger;
    private readonly Dictionary<string, IOperation> _operations;

    public OperationService(BotContext bot, ILogger<LagrangeApp> logger, LagrangeWebSvcCollection service)
    {
        _bot = bot;
        _logger = logger;
        _operations = new Dictionary<string, IOperation>();
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<OperationAttribute>();
            if (attribute != null) _operations[attribute.Api] = (IOperation)type.CreateInstance(false);
        }

        service.OnMessageReceived += (s, e) => _ = HandleOperation(s, e);
    }

    private async Task HandleOperation(object? sender, MsgRecvEventArgs eventArgs)
    {
        if (sender is not ILagrangeWebService webService)
        {
            _logger.LogWarning("Json Serialization failed for such action");
            return;
        }
        if (JsonSerializer.Deserialize<OneBotAction>(eventArgs.Data) is { } action)
        {
            try
            {
                if (_operations.TryGetValue(action.Action, out var handler))
                {
                    var result = await handler.HandleOperation(_bot, action.Params);
                    result.Echo = action.Echo;

                    await webService.SendJsonAsync(result);
                }
                else
                {
                    await webService.SendJsonAsync(new OneBotResult(null, 404, "failed") { Echo = action.Echo });
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unexpected error encountered while handling message.");
                await webService.SendJsonAsync(new OneBotResult(null, 200, "failed") { Echo = action.Echo });
            }
        }
        else
        {
            _logger.LogWarning("Json Serialization failed for such action");
        }
    }
}