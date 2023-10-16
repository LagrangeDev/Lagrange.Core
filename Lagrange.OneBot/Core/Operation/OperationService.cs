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
        _operations = [];

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<OperationAttribute>();
            if (attribute != null)
                _operations[attribute.Api] = (IOperation)type.CreateInstance(false);
        }

        service.OnMessageReceived += async (_, e) => await HandleOperation(e);
    }

    private async Task HandleOperation(MsgRecvEventArgs e)
    {
        var action = JsonSerializer.Deserialize<OneBotAction>(e.Data);

        if (action != null)
        {
            var handler = _operations[action.Action];
            var result = await handler.HandleOperation(action.Echo, _bot, action.Params);

            if (!string.IsNullOrEmpty(e.Identifier)) // add an identifier for `ForwardWSService`
                result.Identifier = e.Identifier;

            await _service.SendJsonAsync(result);
        }
        else
        {
            throw new Exception("action deserialized failed");
        }
    }
}
