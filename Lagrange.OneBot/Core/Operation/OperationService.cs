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
    private readonly LagrangeWebSvcCollection _service;
    private readonly Dictionary<string, IOperation> _operations;

    public OperationService(BotContext bot, ILogger<LagrangeApp> logger, LagrangeWebSvcCollection service)
    {
        _bot = bot;
        _logger = logger;
        _service = service;
        _operations = new Dictionary<string, IOperation>();
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<OperationAttribute>();
            if (attribute != null) _operations[attribute.Api] = (IOperation)type.CreateInstance(false);
        }

        service.OnMessageReceived += async (_, e) => await HandleOperation(e);
    }

    private async Task HandleOperation(MsgRecvEventArgs eventArgs)
    {
        if (JsonSerializer.Deserialize<OneBotAction>(eventArgs.Data) is { } action)
        {
            try
            {
                bool supported = _operations.TryGetValue(action.Action, out var handler);

                if (supported && handler != null)
                {
                    var result = await handler.HandleOperation(_bot, action.Params);
                    result.Echo = action.Echo;

                    await _service.SendJsonAsync(result, eventArgs.Identifier);
                }
                else
                {
                    await _service.SendJsonAsync(new OneBotResult(null, 404, "failed") { Echo = action.Echo }, eventArgs.Identifier);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.ToString());
                await _service.SendJsonAsync(new OneBotResult(null, 200, "failed") { Echo = action.Echo }, eventArgs.Identifier);
            }
        }
        else
        {
            _logger.LogWarning($"Json Serialization failed for such action");
        }
    }
}