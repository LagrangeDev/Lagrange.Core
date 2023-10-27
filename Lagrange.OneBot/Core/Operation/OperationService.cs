using System.Reflection;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Network.Service;

namespace Lagrange.OneBot.Core.Operation;

public sealed class OperationService
{
    private readonly BotContext _bot;
    private readonly LagrangeWebSvcCollection _service;
    private readonly Dictionary<string, IOperation> _operations;

    public OperationService(BotContext bot, LagrangeWebSvcCollection service)
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

        try
        {
            if (action != null)
            {
                bool supported = _operations.TryGetValue(action.Action, out var handler);

                if (supported && handler != null)
                {
                    var result = await handler.HandleOperation(_bot, action.Params);
                    result.Echo = action.Echo;

                    await _service.SendJsonAsync(result);
                }
                else
                {
                    await _service.SendJsonAsync(new OneBotResult(null, 404, "failed"));
                }
            }
            else
            {
                throw new Exception("action deserialized failed");
            }
        }
        catch
        {
            await _service.SendJsonAsync(new OneBotResult(null, 200, "failed"));
        }
    }
}