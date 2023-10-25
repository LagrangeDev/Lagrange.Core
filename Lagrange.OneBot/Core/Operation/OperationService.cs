using System.Reflection;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Network;

namespace Lagrange.OneBot.Core.Operation;

public sealed class OperationService
{
    private readonly BotContext _bot;
    private readonly ILagrangeWebService _service;
    private readonly Dictionary<string, IOperation> _operations;

    public OperationService(BotContext bot, ILagrangeWebService service)
    {
        _bot = bot;
        _service = service;
        _operations = new Dictionary<string, IOperation>();
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<OperationAttribute>();
            if (attribute != null) _operations[attribute.Api] = (IOperation)type.CreateInstance(false);
        }

        service.OnMessageReceived += async (_, e) => await HandleOperation(e.Data);
    }

    private async Task HandleOperation(string data)
    {
        var action = JsonSerializer.Deserialize<OneBotAction>(data);
            
        if (action != null)
        {
            bool supported = _operations.TryGetValue(action.Action, out var handler);
            
            if (supported && handler != null)
            {
                var result = await handler.HandleOperation(action.Echo, _bot, action.Params);
                await _service.SendJsonAsync(result);
            }
        }
        else
        {
            throw new Exception("action deserialized failed");
        }
    }
}