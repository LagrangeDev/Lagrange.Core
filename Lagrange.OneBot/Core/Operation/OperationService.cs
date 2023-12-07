using System.Reflection;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Network;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Operation;

public sealed class OperationService
{
    private readonly BotContext _bot;
    private readonly ILogger _logger;
    private readonly Dictionary<string, IOperation> _operations;

    public OperationService(BotContext bot, ILogger<OperationService> logger, LagrangeWebSvcCollection service)
    {
        _bot = bot;
        _logger = logger;
        _operations = new Dictionary<string, IOperation>();
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<OperationAttribute>();
            if (attribute != null) _operations[attribute.Api] = (IOperation)type.CreateInstance(false);
        }
    }

    public async Task<OneBotResult?> HandleOperation(MsgRecvEventArgs e)
    {
        if (JsonSerializer.Deserialize<OneBotAction>(e.Data) is { } action)
        {
            try
            {
                if (_operations.TryGetValue(action.Action, out var handler))
                {
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

        _logger.LogWarning("Json Serialization failed for such action");
        return null;
    }
}